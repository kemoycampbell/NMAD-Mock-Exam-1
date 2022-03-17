using System.Collections.Generic;
using System.Threading.Tasks;
using Flexify.Models;

namespace Flexify.Repositories
{
    public interface IFilmRepository
    {
        public Task<Film> Create(Film film);
        public Task<IEnumerable<Film>> All();
        public Task<Film> Get(string id);
        public Task<IEnumerable<Cast>> GetCasts(string id);
        public Task<Film> Update(Film film);
        public void Delete(string id);
        public Task<Rating> GetMovieRating(string id);
        public Task<IEnumerable<Film>> Search(string title);

    }
}