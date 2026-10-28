using MovieAPI.Models.Request;
using MovieAPI.Models.Response;


namespace MovieAPI.Services.Interfaces
{
    public interface IActorService
    {
        Task<IList<ActorResponse>> GetAllAsync();

        Task<ActorResponse> GetByIdAsync(int id);

        Task<ActorResponse> CreateAsync(ActorRequest actorRequest);

        Task<ActorResponse> UpdateAsync(int id, ActorRequest actorRequest);

        Task DeleteAsync(int id);
    }
}