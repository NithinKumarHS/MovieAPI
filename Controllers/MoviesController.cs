using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieAPI.Models.Request;
using MovieAPI.Services.Interfaces;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll([FromQuery]int? year)
        {
            var results =await _movieService.GetAllAsync(year);

            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            var result =await _movieService.GetByIdAsync(id);

            return Ok(result);
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody]MovieRequest request)
        {
            var result = await _movieService.CreateAsync(request);

            return CreatedAtAction(nameof(GetById), new {Id = result.Id}, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute]int id, [FromBody]MovieRequest request)
        {
            var result = await _movieService.UpdateAsync(id, request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            await _movieService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("{id}/poster")]
        public async Task<IActionResult> UpdatePoster([FromRoute] int id, IFormFile file)
        {
            var result = await _movieService.UpdatePosterAsync(id, file);

            return Ok(result);
        }
    }
}