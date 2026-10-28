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
    public class ProducerService : IProducerService
    {
        private readonly IProducerRepository _producerRepository;
        private readonly IMapper _mapper;

        public ProducerService(IProducerRepository producerRepository, IMapper mapper)
        {
            _producerRepository = producerRepository;
            _mapper = mapper;
        }
        public async Task<IList<ProducerResponse>> GetAllAsync()
        {
            var producers = await _producerRepository.GetAllAsync();

            return _mapper.Map<IList<ProducerResponse>>(producers);
        }
        public async Task<ProducerResponse?> GetByIdAsync(int id)
        {
            if(id <= 0)
            {
                throw new ProducerValidationException("Producer Id is Invalid.");
            }

            var producer =await _producerRepository.GetByIdAsync(id);

            if (producer == null)
            {
                throw new KeyNotFoundException("Producer Id is Invalid.");
            }

            return _mapper.Map<ProducerResponse>(producer);
        }

        public async Task<ProducerResponse> CreateAsync(ProducerRequest producer)
        {
            ValidateProducer(producer);

            var producerExist = await _producerRepository.GetByNameDateOfBirthAsync(producer.Name, producer.DateOfBirth);

            if (producerExist != null)
            {
                throw new ProducerValidationException("Producer Already Exist");
            }

            var P =await _producerRepository.CreateAsync(_mapper.Map<Producer>(producer));

            return _mapper.Map<ProducerResponse>(P);
        }

        public async Task<ProducerResponse?> UpdateAsync(int id, ProducerRequest producer)
        {
            if (id <= 0)
            {
                throw new ProducerValidationException("Producer Id is Invalid.");
            }

            ValidateProducer(producer);

            var P = _mapper.Map<Producer>(producer);
            P.Id = id;

            var result =await _producerRepository.UpdateAsync(P);

            if (result == null)
            {
                throw new KeyNotFoundException("Producer Id provided is not valid for updation");
            }

            return _mapper.Map<ProducerResponse>(result);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ProducerValidationException("Producer Id is Invalid.");
            }

            var result = await _producerRepository.DeleteAsync(id);

            if (result == false)
            {
                throw new KeyNotFoundException($"Producer Id {id} is NotFound for Deletion.");
            }
        }

        private void ValidateProducer(ProducerRequest producerRequest)
        {
            if (producerRequest == null)
            {
                throw new ProducerValidationException("Producer Object Cannot be Null.");
            }

            if (string.IsNullOrWhiteSpace(producerRequest.Name))
            {
                throw new ProducerValidationException("Producer Name Cannot be Null or WhiteSpace.");
            }

            producerRequest.Name = producerRequest.Name.Trim();

            if (producerRequest.Name.Length < 2 || producerRequest.Name.Length > 100)
            {
                throw new ProducerValidationException("Name must be between 2 and 100 characters.");
            }

            if (!Regex.IsMatch(producerRequest.Name, @"^[a-zA-Z]+[a-zA-Z\s-.']*$"))
            {
                throw new ProducerValidationException("Name contains invalid characters.");
            }

            if (string.IsNullOrWhiteSpace(producerRequest.Biography))
            {
                throw new ProducerValidationException("Producer Biography Cannot be Null or WhiteSpaces.");
            }

            producerRequest.Biography = producerRequest.Biography.Trim();

            if (producerRequest.Biography.Length > 500)
            {
                throw new ProducerValidationException("Biography cannot exceed 500 characters.");
            }

            if (producerRequest.DateOfBirth == DateOnly.MinValue || producerRequest.DateOfBirth > DateOnly.FromDateTime(DateTime.Now))
            {
                throw new ProducerValidationException("Invalid DateOfBirth.");
            }

            if (!Enum.IsDefined(typeof(Gender), producerRequest.Gender))
            {
                throw new ProducerValidationException("The provided Gender ID is not valid.");
            }
        }
    }
}
