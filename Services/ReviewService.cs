using AutoMapper;
using MovieAPI.Models.DB;
using MovieAPI.Models.Request;
using MovieAPI.Models.Response;
using MovieAPI.Repositories.Interfaces;
using MovieAPI.Services.Exceptions;
using MovieAPI.Services.Interfaces;

namespace MovieAPI.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IMovieRepository _movieRepository;

        public ReviewService(
            IReviewRepository reviewRepository, 
            IMapper mapper, 
            IMovieRepository movieRepository)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _movieRepository = movieRepository;
        }

        public async Task<IList<ReviewResponse>> GetAllAsync()
        {
            var reviews = await _reviewRepository.GetAllAsync();

            return _mapper.Map<IList<ReviewResponse>>(reviews);
        }

        public async Task<ReviewResponse?> GetByIdAsync(int id)
        {
            if(id <= 0)
            {
                throw new ReviewValidationException("Invalid Review Id");
            }

            var review = await _reviewRepository.GetByIdAsync(id);

            if(review == null)
            {
                throw new KeyNotFoundException("Review Id is not Exist.");
            }

            return _mapper.Map<ReviewResponse>(review);
        } 

        public async Task<ReviewResponse> CreateAsync(ReviewRequest reviewRequest)
        {
            ValidateReview(reviewRequest);

            var movieExists = await _movieRepository.GetByIdAsync(reviewRequest.MovieId);

            if (movieExists == null)
            {
                throw new KeyNotFoundException("MovieId is Not Exists.");
            }

            var review = await _reviewRepository.CreateAsync(_mapper.Map<Review>(reviewRequest));

            return _mapper.Map<ReviewResponse>(review);
        }

        public async Task<ReviewResponse?> UpdateAsync(int id, ReviewRequest reviewRequest)
        {
            if (id <= 0)
            {
                throw new ReviewValidationException("Invalid Review Id");
            }

            ValidateReview(reviewRequest);

            var movieExists = await _movieRepository.GetByIdAsync(reviewRequest.MovieId);

            if (movieExists == null)
            {
                throw new KeyNotFoundException("MovieId is Not Exists.");
            }

            var review = _mapper.Map<Review>(reviewRequest);
            review.Id = id;

            var updatedReview = await _reviewRepository.UpdateAsync(review);

            if(updatedReview == null)
            {
                throw new KeyNotFoundException("Review Id is NotFound for Updation.");
            }

            return _mapper.Map<ReviewResponse>(updatedReview);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ReviewValidationException("Invalid Review Id");
            }

            var result = await _reviewRepository.DeleteAsync(id);

            if(result == false)
            {
                throw new KeyNotFoundException($"Review Id {id} is NotFound for Deletion.");
            }
        }

        private void ValidateReview(ReviewRequest reviewRequest)
        {
            if(reviewRequest == null)
            {
                throw new ReviewValidationException("Review Object Cannot be Null.");
            }

            if (string.IsNullOrWhiteSpace(reviewRequest.Message))
            {
                throw new ReviewValidationException("Review Message Cannot be Null or WhiteSpace.");
            }

            reviewRequest.Message = reviewRequest.Message.Trim();

            if (reviewRequest.Message.Length < 5 || reviewRequest.Message.Length > 1000)
            {
                throw new ReviewValidationException("Review Message must be between 5 and 1000 characters.");
            }

            if(reviewRequest.MovieId <= 0)
            {
                throw new ReviewValidationException("Invalid MovieId");
            }
        }
    }
}
