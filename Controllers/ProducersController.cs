using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieAPI.Models.Request;
using MovieAPI.Services.Interfaces;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProducersController : ControllerBase
    {
        private readonly IProducerService _producerService;

        public ProducersController(IProducerService producerService)
        {
            _producerService = producerService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var results = await _producerService.GetAllAsync();

            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _producerService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] ProducerRequest producerRequest)
        {

            var producer = await _producerService.CreateAsync(producerRequest);
            return CreatedAtAction(nameof(GetById), new { id = producer.Id }, producer);
            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ProducerRequest producerRequest)
        {
            var producer =await _producerService.UpdateAsync(id, producerRequest);
            return Ok(producer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _producerService.DeleteAsync(id);
            return NoContent();
        }
    }
}
