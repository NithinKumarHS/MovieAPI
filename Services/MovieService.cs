using AutoMapper;
using MovieAPI.Models.DB;
using MovieAPI.Models.Request;
using MovieAPI.Models.Response;
using MovieAPI.Repositories.Interfaces;
using MovieAPI.Services.Exceptions;
using MovieAPI.Services.Interfaces;

namespace MovieAPI.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IProducerRepository _producerRepository;
        private readonly IActorRepository _actorRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;
        private readonly ISupabaseStorageService _supabaseStorageService;

        public MovieService(
            IMovieRepository movieRepository,
            IProducerRepository producerRepository,
            IActorRepository actorRepository,
            IGenreRepository genreRepository,
            IMapper mapper,
            ISupabaseStorageService supabaseStorageService)
        {
            _movieRepository = movieRepository;
            _producerRepository = producerRepository;
            _actorRepository = actorRepository;
            _genreRepository = genreRepository;
            _mapper = mapper;
            _supabaseStorageService = supabaseStorageService;
        }
        public async Task<IList<MovieResponse>> GetAllAsync(int? year)
        {
            var movies = await _movieRepository.GetAllAsync();

            if (year.HasValue)
            {
                movies = movies
                            .Where(movie => movie.YearOfRelease == year.Value)
                            .ToList();
            }

            var responses = new List<MovieResponse>();

            foreach (var movie in movies)
            {
                responses.Add( await BuildResponseAsync(movie));
            }

            return responses;
        }

        public async Task<MovieResponse> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new MovieValidationException("Movie Id is Invalid.");
            }

            var movie = await _movieRepository.GetByIdAsync(id);

            if(movie == null)
            {
                throw new KeyNotFoundException("MovieId Not Exist");
            }

            return await BuildResponseAsync(movie);
        }

        public async Task<MovieResponse> CreateAsync(MovieRequest request)
        {
            ValidateRequest(request);

            var existMovie = await _movieRepository.GetByNameYearAsync(request.Name, request.YearOfRelease);
            
            if (existMovie != null)
            {
                throw new MovieValidationException("Movie Already Exist.");
            }

            await ValidateProducer(request.ProducerId);
            await ValidateActors(request.ActorIds);
            await ValidateGenres(request.GenreIds);

            var movie = _mapper.Map<Movie>(request);

            await _movieRepository.CreateAsync(movie, request.ActorIds, request.GenreIds);

            return await BuildResponseAsync(movie);
        }

        public async Task<MovieResponse> UpdateAsync(int id, MovieRequest request)
        {
            if (id <= 0)
            {
                throw new MovieValidationException("Movie Id is Invalid.");
            }

            var existMovie = await _movieRepository.GetByIdAsync(id);

            if(existMovie == null)
            {
                throw new KeyNotFoundException($"Movie Id {id} does not exist.");
            }

            ValidateRequest(request);
            await ValidateProducer(request.ProducerId);
            await ValidateActors(request.ActorIds);
            await ValidateGenres(request.GenreIds);

            var movie = _mapper.Map<Movie>(request);
            movie.Id = id;

            await _movieRepository.UpdateAsync(movie, request.ActorIds, request.GenreIds);

            return await BuildResponseAsync(movie);
        }


        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new MovieValidationException("Movie Id is Invalid.");
            }

            var movie = await _movieRepository.GetByIdAsync(id);

            if(movie == null)
            {
                throw new KeyNotFoundException($"Movie Id {id} Not Exist.");
            }

            await _movieRepository.DeleteAsync(id);

        }


        private void ValidateRequest(MovieRequest request)
        {
            if (request == null)
            {
                throw new MovieValidationException("Movie Object Cannot be Null.");
            }

            // Movie Name
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new MovieValidationException("Movie Name Cannot be Null or WhiteSpace.");
            }

            request.Name = request.Name.Trim();

            if (request.Name.Length < 2 || request.Name.Length > 200)
            {
                throw new MovieValidationException("Movie Name must be between 2 and 200 characters.");
            }

            // Year Of Release
            if (request.YearOfRelease < 1888 || request.YearOfRelease > DateOnly.FromDateTime(DateTime.Now).Year)
            {
                throw new MovieValidationException("YearOfRelease is Invalid.");
            }

            // Plot
            if (string.IsNullOrWhiteSpace(request.Plot))
            {
                throw new MovieValidationException("Movie Plot Cannot be Null or WhiteSpace.");
            }

            request.Plot = request.Plot.Trim();

            if (request.Plot.Length < 10 || request.Plot.Length > 2000)
            {
                throw new MovieValidationException("Movie Plot must be between 10 and 2000 characters.");
            }

            // Producer
            if (request.ProducerId <= 0)
            {
                throw new MovieValidationException("Invalid ProducerId.");
            }

            // Cover Image
            if (string.IsNullOrWhiteSpace(request.CoverImage))
            {
                throw new MovieValidationException("Cover Image Cannot be Null or WhiteSpace.");
            }

            request.CoverImage = request.CoverImage.Trim();

            // Actors
            if (request.ActorIds == null || request.ActorIds.Count == 0)
            {
                throw new MovieValidationException("Atleast One Actor is Required.");
            }

            if (request.ActorIds.Any(id => id <= 0))
            {
                throw new MovieValidationException("ActorIds must contain valid IDs.");
            }

            if (request.ActorIds.Distinct().Count() != request.ActorIds.Count)
            {
                throw new MovieValidationException("Duplicate ActorIds are not allowed.");
            }

            // Genres
            if (request.GenreIds == null || request.GenreIds.Count == 0)
            {
                throw new MovieValidationException("Atleast One GenreId is Required.");
            }

            if (request.GenreIds.Any(id => id <= 0))
            {
                throw new MovieValidationException("GenreIds must contain valid IDs.");
            }

            if (request.GenreIds.Distinct().Count() != request.GenreIds.Count)
            {
                throw new MovieValidationException("Duplicate GenreIds are not allowed.");
            }
        }


        private async Task ValidateProducer(int producerId)
        {
            var existProducer = await _producerRepository.GetByIdAsync(producerId);

            if (existProducer == null)
            {
                throw new KeyNotFoundException($"Producer with Id {producerId} Not Exist");
            }
        }

        private async Task ValidateActors(List<int> actorIds)
        {
            foreach (var actorId in actorIds)
            {
                var actor = await _actorRepository.GetByIdAsync(actorId);

                if (actor == null)
                {
                    throw new KeyNotFoundException($"Actor with Id {actorId} Not Exist");
                }
            }
        }

        private async Task ValidateGenres(List<int> genreIds)
        {
            foreach (var genreId in genreIds)
            {
                var genre = await _genreRepository.GetByIdAsync(genreId);

                if (genre == null)
                {
                    throw new KeyNotFoundException($"Genre with Id {genreId} Not Exist");
                }
            }
        }

        private async Task<MovieResponse> BuildResponseAsync(Movie movie)
        {

            //Actor Object Generation
            var actorIds = await _movieRepository.GetActorIdsAsync(movie.Id);

            var actors = new List<Actor>();

            foreach (var actorId in actorIds)
            {
                var actor = await _actorRepository.GetByIdAsync(actorId);
                if (actor != null)
                {
                    actors.Add(actor);
                }
            }

            var actorResponse = _mapper.Map<List<ActorResponse>>(actors);


            //Genre Object Generation

            var genreIds = await _movieRepository.GetGenreIdsAsync(movie.Id);

            var genres = new List<Genre>();


            foreach (var genreId in genreIds)
            {
                var genre = await _genreRepository.GetByIdAsync(genreId);

                if (genre != null)
                {
                    genres.Add(genre);
                }
            }

            var genreResponse = _mapper.Map<List<GenreResponse>>(genres);


            //Producer Object Generation

            var producer = await _producerRepository.GetByIdAsync(movie.ProducerId);
            var proudcerResponse = new ProducerResponse();
            if (producer != null)
            {
                proudcerResponse = _mapper.Map<ProducerResponse>(producer);
            }

            var movieResponse = _mapper.Map<MovieResponse>(movie);
            movieResponse.Producer = proudcerResponse;
            movieResponse.Actors = actorResponse;
            movieResponse.Genres = genreResponse;

            return movieResponse;
        }

        public async Task<MovieResponse> UpdatePosterAsync(int movieId,  IFormFile poster)
        {
            var movieExist = await _movieRepository.GetByIdAsync(movieId);

            if(movieExist == null)
            {
                throw new KeyNotFoundException($"Movie Id {movieId} is Not Exist.");
            }

            var fileExtension = Path.GetExtension(poster.FileName);

            var fileName = $"{movieId}{fileExtension}";

            var imageUrl = await _supabaseStorageService.UploadAsync(poster, fileName);

            var updatedMovie = await _movieRepository.UpdateCoverImageAsync(movieId, imageUrl);

            if(updatedMovie == false)
            {
                throw new MovieValidationException("Failed to Update Movie Poster.");
            }

            movieExist.CoverImage = imageUrl;

            return await BuildResponseAsync(movieExist);
        }


    }
}
