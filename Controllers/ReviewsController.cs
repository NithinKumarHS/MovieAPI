using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieAPI.Models.Request;
using MovieAPI.Services.Interfaces;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var results = await _reviewService.GetAllAsync();
            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            var result = await _reviewService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody]ReviewRequest reviewRequest)
        {
            var result = await _reviewService.CreateAsync(reviewRequest);
            return CreatedAtAction(nameof(GetById), new { Id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute]int id, [FromBody]ReviewRequest reviewRequest)
        {
            var result = await _reviewService.UpdateAsync(id, reviewRequest);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            await _reviewService.DeleteAsync(id);
            return NoContent();
        }
    }
}
