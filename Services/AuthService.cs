using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using MovieAPI.Models.DB;
using MovieAPI.Models.Request;
using MovieAPI.Models.Response;
using MovieAPI.Repositories.Interfaces;
using MovieAPI.Services.Exceptions;
using MovieAPI.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace MovieAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthService(IAuthRepository userRepository, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<IList<SignUpResponse>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return _mapper.Map<IList<SignUpResponse>>(users);
        }

        public async Task<SignUpResponse> CreateAsync(SignUpRequest signUpRequest)
        {
            if(signUpRequest == null)
            {
                throw new SignUpValidationException("Signup Object Cannot be null");
            }

            if (string.IsNullOrWhiteSpace(signUpRequest.Name))
            {
                throw new SignUpValidationException("Signup Name Cannot be null or whiteSpace.");
            }

            signUpRequest.Name = signUpRequest.Name.Trim();

            if (signUpRequest.Name.Length < 2 || signUpRequest.Name.Length > 100)
            {
                throw new SignUpValidationException("Name must be between 2 and 100 characters.");
            }

            if (!Regex.IsMatch(signUpRequest.Name, @"^[a-zA-Z]+[a-zA-Z\s-.']*$"))
            {
                throw new SignUpValidationException("Name contains invalid characters.");
            }

            if (string.IsNullOrWhiteSpace(signUpRequest.Password))
            {
                throw new SignUpValidationException("Password Cannot be Null or WhiteSpace.");
            }

            if (signUpRequest.Password.Length < 8)
            {
                throw new SignUpValidationException("Password Length must be 8 character.");
            }

            var existUser = await _userRepository.GetUserAsync(signUpRequest.Email);

            if (existUser != null)
            {
                throw new SignUpValidationException("User Already Exist.");
            }

            var user = await _userRepository.CreateAsync(_mapper.Map<User>(signUpRequest));

            return _mapper.Map<SignUpResponse>(user);
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
            var validUser = await _userRepository.GetUserAsync(loginRequest.Email);

            if (validUser == null || validUser.Password != loginRequest.Password)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            string token = GenerateToken(validUser.Email);

            var Response = _mapper.Map<LoginResponse>(validUser);
            Response.AccessToken = token;

            return Response;
        }

        private string GenerateToken(string email)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString())
            };

            var authSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtToken:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtToken:ValidIssuer"],
                audience: _configuration["JwtToken:ValidAudience"],
                expires: DateTime.Now.AddMinutes(20),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}