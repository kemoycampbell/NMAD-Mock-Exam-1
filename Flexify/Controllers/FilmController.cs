using System.Threading.Tasks;
using Flexify.Models;
using Flexify.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flexify.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/films/")]
    public class FilmController : ControllerBase
    {
        private readonly IFilmRepository _repository;

        public FilmController(IFilmRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public async Task<IActionResult> AllFilms()
        {
            return Ok(await _repository.All());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFilmById(string id)
        {
            return Ok(await _repository.Get(id));
        }

        [HttpGet("{id}/casts")]
        public async Task<IActionResult> GetFilmCast(string id)
        {
            return Ok(await _repository.GetCasts(id));
        }

        [HttpGet("{id}/ratings")]
        public async Task<IActionResult> GetRating(string id)
        {
            return Ok(await _repository.GetMovieRating(id));
        }

        [HttpGet("search/title/{keyword}")]
        public async Task<IActionResult> SearchByTitle(string keyword)
        {
            return Ok(await _repository.Search(keyword));
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateFilm(Film film)
        {
            return Ok(await _repository.Update(film));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFilm(string id)
        {
            _repository.Delete(id);
            return Ok("Movie successful deleted");
        }

        [HttpPost]
        public async Task<IActionResult> Create(Film film)
        {
            return Ok(await _repository.Create(film));
        }
    }
}