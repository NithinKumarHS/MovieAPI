using MovieAPI.Models.Request;
using MovieAPI.Models.Response;

namespace MovieAPI.Services.Interfaces
{
    public interface IProducerService
    {
        Task<IList<ProducerResponse>> GetAllAsync();
        Task<ProducerResponse?> GetByIdAsync(int id);
        Task<ProducerResponse> CreateAsync(ProducerRequest producerRequest);
        Task<ProducerResponse?> UpdateAsync(int id, ProducerRequest producerRequest);
        Task DeleteAsync(int id);
    }
}
