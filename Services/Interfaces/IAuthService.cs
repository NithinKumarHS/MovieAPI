using MovieAPI.Models.Request;
using MovieAPI.Models.Response;

namespace MovieAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<IList<SignUpResponse>> GetAllAsync();

        Task<SignUpResponse> CreateAsync(SignUpRequest signUpRequest);

        Task<LoginResponse> LoginAsync(LoginRequest loginRequest);

    }
}
