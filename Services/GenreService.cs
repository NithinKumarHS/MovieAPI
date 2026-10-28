using AutoMapper;
using MovieAPI.Models.DB;
using MovieAPI.Models.Request;
using MovieAPI.Models.Response;
using MovieAPI.Repositories.Interfaces;
using MovieAPI.Services.Exceptions;
using MovieAPI.Services.Interfaces;
using System.Text.RegularExpressions;

namespace MovieAPI.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public GenreService(IGenreRepository genreRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        public async Task<IList<GenreResponse>> GetAllAsync()
        {
            var genres = await _genreRepository.GetAllAsync();
            return _mapper.Map<IList<GenreResponse>>(genres);
        }

        public async Task<GenreResponse?> GetByIdAsync(int id)
        {
            if(id <= 0)
            {
                throw new GenreValidationException("Genre Id is Invalid.");
            }

            var genre = await _genreRepository.GetByIdAsync(id);

            if(genre  == null)
            {
                throw new KeyNotFoundException("GenreId not Exist.");
            }

            return _mapper.Map<GenreResponse>(genre);
        }

        public async Task<GenreResponse> CreateAsync(GenreRequest genreRequest)
        {
            ValidateGenre(genreRequest);
            
            var genre = await _genreRepository.CreateAsync(_mapper.Map<Genre>(genreRequest));
            return _mapper.Map<GenreResponse>(genre);
        }

        public async Task<GenreResponse?> UpdateAsync(int id, GenreRequest genreRequest)
        {
            if (id <= 0)
            {
                throw new GenreValidationException("Genre Id is Invalid.");
            }

            ValidateGenre(genreRequest);

            var genre = _mapper.Map<Genre>(genreRequest);
            genre.Id = id;

            var genreExist = await _genreRepository.UpdateAsync(genre);

            if(genreExist == null)
            {
                throw new KeyNotFoundException("GenreId is Not Exist for Updation.");
            }

            return _mapper.Map<GenreResponse>(genreExist);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new GenreValidationException("Genre Id is Invalid.");
            }

            var result = await _genreRepository.DeleteAsync(id);

            if(result == false)
            {
                throw new KeyNotFoundException("GenreId is Not Exist for Deletion.");
            }
        }

        private void ValidateGenre(GenreRequest genreRequest)
        {
            if (genreRequest == null)
            {
                throw new GenreValidationException("Genre Object Cannot be Null.");
            }

            if (string.IsNullOrWhiteSpace(genreRequest.Name))
            {
                throw new GenreValidationException("Genre Name Cannot be Null or WhiteSpace.");
            }

            genreRequest.Name = genreRequest.Name.Trim();

            if (genreRequest.Name.Length < 2 || genreRequest.Name.Length > 50)
            {
                throw new GenreValidationException("Genre Name must be between 2 and 50 characters.");
            }

            if (!Regex.IsMatch(genreRequest.Name, @"^[a-zA-Z]+[a-zA-Z\s&-]*$"))
            {
                throw new GenreValidationException("Genre Name contains invalid characters.");
            }
        }
    }
}
