using AutoMapper;
using MovieAPI.Models;
using MovieAPI.Models.DB;
using MovieAPI.Models.Request;
using MovieAPI.Models.Response;

namespace MovieAPI.Helper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {

            CreateMap<ActorRequest, Actor>()
                .ForMember(
                    dest => dest.GenderId,
                    opt => opt.MapFrom(src => (int)src.Gender)
                );

            CreateMap<Actor, ActorResponse>()
                .ForMember(
                    dest => dest.Gender,
                    opt => opt.MapFrom(src => (Gender)src.GenderId)
                );


            CreateMap<ProducerRequest, Producer>()
                .ForMember(
                    dest => dest.GenderId,
                    opt => opt.MapFrom(src => (int)src.Gender)
                );

            CreateMap<Producer, ProducerResponse>()
                .ForMember(
                    dest => dest.Gender,
                    opt => opt.MapFrom(src => (Gender)src.GenderId)
                );

            CreateMap<Review, ReviewResponse>();
            CreateMap<ReviewRequest, Review>();

            CreateMap<Genre,  GenreResponse>();
            CreateMap<GenreRequest, Genre>();

            CreateMap<Movie, MovieResponse>();
            CreateMap<MovieRequest, Movie>();

            CreateMap<User, SignUpResponse>();
            CreateMap<SignUpRequest, User>();

            CreateMap<LoginRequest, User>();
            CreateMap<User, LoginResponse>();
        }
    }
}
