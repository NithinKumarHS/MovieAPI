using AutoMapper;
using MovieAPI.Models;
using MovieAPI.Models.DB;
using MovieAPI.Models.Request;
using MovieAPI.Models.Response;
using MovieAPI.Repositories.Interfaces;
using MovieAPI.Services.Exceptions;
using MovieAPI.Services.Interfaces;
using System.Text.RegularExpressions;

namespace MovieAPI.Services
{
    public class ActorService : IActorService
    {
        private readonly IActorRepository _actorRepository;
        private readonly IMapper _mapper;

        public ActorService(IActorRepository actorRepository, IMapper mapper)
        {
            _actorRepository = actorRepository;
            _mapper = mapper;
        }

        public async Task<IList<ActorResponse>> GetAllAsync()
        {
            var actors = await _actorRepository.GetAllAsync();

            return _mapper.Map<IList<ActorResponse>>(actors);
        }

        public async Task<ActorResponse> GetByIdAsync(int id)
        {
            if(id <= 0)
            {
                throw new ActorValidationException("Actor Id is Invalid.");
            }

            var actor = await _actorRepository.GetByIdAsync(id);

            if (actor == null)
            {
                throw new KeyNotFoundException("Actor Id Not Exist.");
            }

            return _mapper.Map<ActorResponse>(actor);
        }

        public async Task<ActorResponse> CreateAsync(ActorRequest actor)
        {
            ValidateActor(actor);

            var actorExist = await _actorRepository.GetByNameDateOfBirthAsync( actor.Name, actor.DateOfBirth);

            if (actorExist != null)
            {
                throw new ActorValidationException("Actor Already Exist");
            }

            var actorEntity = await _actorRepository.CreateAsync(_mapper.Map<Actor>(actor));

            return _mapper.Map<ActorResponse>(actorEntity);
        }

        public async Task<ActorResponse> UpdateAsync(int id, ActorRequest actor)
        {
            if(id <= 0)
            {
                throw new ActorValidationException("Actor Id is Invalid.");
            }

            ValidateActor(actor);

            var actorEntity = _mapper.Map<Actor>(actor);
            actorEntity.Id = id;

            var result = await _actorRepository.UpdateAsync(actorEntity);

            if (result == null)
            {
                throw new KeyNotFoundException("Actor Id provided is not valid for updation");
            }

            return _mapper.Map<ActorResponse>(result);
        }

        public async Task DeleteAsync(int id)
        {
            if(id <= 0)
            {
                throw new ActorValidationException("Actor Id is Invalid.");
            }

            var result = await _actorRepository.DeleteAsync(id);

            if (result == false)
            {
                throw new KeyNotFoundException($"Actor Id {id} is NotFound for Deletion.");
            }
        }

        private void ValidateActor(ActorRequest actorRequest)
        {
            if (actorRequest == null)
            {
                throw new ActorValidationException("Actor Object Cannot be Null.");
            }

            if (string.IsNullOrWhiteSpace(actorRequest.Name))
            {
                throw new ActorValidationException("Actor Name Cannot be Null or WhiteSpace.");
            }

            actorRequest.Name = actorRequest.Name.Trim();

            if (actorRequest.Name.Length < 2 || actorRequest.Name.Length > 100)
            {
                throw new ActorValidationException("Name must be between 2 and 100 characters.");
            }

            if (!Regex.IsMatch(actorRequest.Name,@"^[a-zA-Z]+[a-zA-Z\s-.']*$"))
            {
                throw new ActorValidationException("Name contains invalid characters.");
            }

            if (string.IsNullOrWhiteSpace(actorRequest.Biography))
            {
                throw new ActorValidationException("Actor Biography Cannot be Null or WhiteSpaces.");
            }

            actorRequest.Biography = actorRequest.Biography.Trim();

            if (actorRequest.Biography.Length > 500)
            {
                throw new ActorValidationException("Biography cannot exceed 500 characters.");
            }

            if (actorRequest.DateOfBirth == DateOnly.MinValue || actorRequest.DateOfBirth > DateOnly.FromDateTime(DateTime.Now))
            {
                throw new ActorValidationException("Invalid DateOfBirth");
            }


            if (!Enum.IsDefined(typeof(Gender), actorRequest.Gender))
            {
                throw new ActorValidationException("The provided Gender ID is not valid.");
            }
        }
    }
}