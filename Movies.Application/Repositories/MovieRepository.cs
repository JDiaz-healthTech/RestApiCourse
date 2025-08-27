using Movies.Application.Models;
using Movies.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Application.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly List<Movie> _movies = new();

        public Task<bool> CreateAsync(Movie movie)
        {
            _movies.Add(movie);
            return Task.FromResult(true);
        }

        public Task<Movie?> GetByIdAsync(Guid id)
        {
            var movie = _movies.SingleOrDefault(x => x.Id == id);
            return Task.FromResult(movie);
        }

        public Task<IEnumerable<Movie>> GetAllAsync()
        {
            // En in-memory no hace falta loguear aquí; lo dejo simple
            return Task.FromResult(_movies.AsEnumerable());
        }

        public Task<bool> UpdateAsync(Movie movie)
        {
            var movieIndex = _movies.FindIndex(x => x.Id == movie.Id);
            if (movieIndex == -1)
            {
                return Task.FromResult(false);
            }

            _movies[movieIndex] = movie;
            return Task.FromResult(true);
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            var removedCount = _movies.RemoveAll(x => x.Id == id);
            var movieRemoved = removedCount > 0;
            return Task.FromResult(movieRemoved);
        }

        // PATCH: aplica cambios parciales al recurso
        public Task<bool> PatchByIdAsync(Guid id, MoviePartialUpdateRequest patch)
        {
            var movieIndex = _movies.FindIndex(x => x.Id == id);
            if (movieIndex == -1)
            {
                return Task.FromResult(false);
            }

            var movie = _movies[movieIndex];

            // OJO: asegúrate de que en tu DTO la propiedad sea 'Title' (no 'Tittle')
            if (patch.Tittle is not null)
                movie.Title = patch.Tittle;

            if (patch.YearOfRelease.HasValue)
                movie.YearOfRelease = patch.YearOfRelease.Value;

            if (patch.Genres is not null)
            {
                // Merge inteligente: limpia y unifica (case-insensitive)
                var newGenres = new HashSet<string>(
                    patch.Genres.Select(g => g?.Trim() ?? string.Empty)
                                .Where(g => g.Length > 0),
                    StringComparer.OrdinalIgnoreCase);

                var oldGenres = new HashSet<string>(movie.Genres, StringComparer.OrdinalIgnoreCase);

                // Agregar los que faltan
                foreach (var g in newGenres.Except(oldGenres))
                    movie.Genres.Add(g);

                // Quitar los que ya no están
                foreach (var g in oldGenres.Except(newGenres).ToList())
                    movie.Genres.Remove(g);
            }

            // En lista in-memory, con mutar 'movie' es suficiente; reasignar es opcional
            _movies[movieIndex] = movie;
            return Task.FromResult(true);
        }
    }
}
