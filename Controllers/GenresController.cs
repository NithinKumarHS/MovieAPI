using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieAPI.Models.Request;
using MovieAPI.Services.Interfaces;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        
        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var results = await _genreService.GetAllAsync();
            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _genreService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody]GenreRequest request)
        {
            var result = await _genreService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute]int id, [FromBody]GenreRequest request)
        {
            var result = await _genreService.UpdateAsync(id, request); 
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            await _genreService.DeleteAsync(id);
            return NoContent();
        }
    }
}
