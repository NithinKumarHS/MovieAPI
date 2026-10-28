using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieAPI.Models.Request;
using MovieAPI.Services.Interfaces;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ActorsController : ControllerBase
    {
        private readonly IActorService _actorService;

        public ActorsController(IActorService actorService)
        {
            _actorService = actorService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var results = await _actorService.GetAllAsync();

            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _actorService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] ActorRequest actorRequest)
        {
            var actor = await _actorService.CreateAsync(actorRequest);

            return CreatedAtAction(
                nameof(GetById),
                new { id = actor.Id },
                actor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            [FromRoute] int id,
            [FromBody] ActorRequest actorRequest)
        {
            var actor = await _actorService.UpdateAsync(
                id,
                actorRequest);

            return Ok(actor);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _actorService.DeleteAsync(id);

            return NoContent();
        }
    }
}