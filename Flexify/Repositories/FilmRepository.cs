using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flexify.Exceptions;
using Flexify.Models;
using Microsoft.EntityFrameworkCore;
namespace Flexify.Repositories
{
    public class FilmRepository : IFilmRepository
    {
        private readonly DatabaseContext _database;

        public FilmRepository(DatabaseContext database)
        {
            _database = database;
        }
        
        public async Task<Film> Create(Film film)
        {
            if (film is null)
                throw new UserException("film cannot be empty");
            await _database.Films.AddAsync(film);
            await _database.SaveChangesAsync();
            return film;
        }

        public async Task<IEnumerable<Film>> All()
        {
            return await _database.Films.ToListAsync();
        }

        public async Task<Film> Get(string id)
        {
            return await _database.Films.FirstOrDefaultAsync(film => film.Id == id);
        }

        public async Task<IEnumerable<Cast>> GetCasts(string id)
        {
            List<Cast> casts = new List<Cast>();

            Film film = await Get(id);
            //if(film is null || film.Cast is null)
            if (film?.Cast is null)
                return casts;
            
            foreach (string member in film.Cast.Split(","))
                casts.Add(new Cast {Name = member});
            return casts;
            
    
        }

        public async Task<Film> Update(Film film)
        {
            _database.Films.Update(film);
            await _database.SaveChangesAsync();
            return film;
        }

        public async void Delete(string id)
        {
            Film film = await Get(id);
            _database.Films.Remove(film);
            await _database.SaveChangesAsync();
        }

        public async Task<Rating> GetMovieRating(string id)
        {
            Film film = await Get(id);
            Rating rating = new Rating();
            rating.Rate = film.Rating;
            rating.Title = film.Title;
            return rating;
        }

        public async Task<IEnumerable<Film>>Search(string title)
        {
            //lower the tile so it is easy to compare
            title = title.ToLower();
            return await _database.Films.Where(film => film.Title.ToLower().Contains(title)).ToListAsync();
        }
    }
}