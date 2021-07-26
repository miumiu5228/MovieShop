
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.RepositoryInterfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class MovieRepository : EfRepository<Movie>, IMovieRepository
    {
        public MovieRepository(MovieShopDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Movie>> GetHighest30GrossingMovies()
        {
           
            var movies = await _dbContext.Movies.OrderByDescending(m => m.Revenue).Take(30).ToListAsync();
            return movies;
        }

        public override async Task<Movie> GetByIdAsync(int id)
        {
            var movie = await _dbContext.Movies.Include(m => m.MovieCasts).ThenInclude(m => m.Cast)
                .Include(m => m.Genres).FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                throw new Exception($"No Movie Found with {id}");
            }

            var movieRating = await _dbContext.Reviews.Where(m => m.MovieId == id).AverageAsync(r => r == null ? 0 : r.Rating);

            if (movieRating > 0)
            {
                movie.Rating = movieRating;
            }

            return movie;
        }

        public async Task<List<Movie>> GetMoviesByGenre(int id)
        {


            var movies = await _dbContext.Movies.Include(m => m.Genres).ToListAsync();
    


            return movies;

        }

        public async Task<List<Movie>> GetMoviesCast()
        {
            var movies = await _dbContext.Movies.Include(m => m.MovieCasts).ToListAsync();
            return movies;
        }

        public async Task<IEnumerable<Movie>> GetTopRatedMovies()
        {
            var topRatedMovies = await _dbContext.Reviews.Include(m => m.Movie)
                    .GroupBy(r => new
                    {
                        Id = r.MovieId,
                        r.Movie.PosterUrl,
                        r.Movie.Title,
                        r.Movie.ReleaseDate
                    })
                    .OrderByDescending(g => g.Average(m => m.Rating))
                    .Select(m => new Movie
                    {
                        Id = m.Key.Id,
                        PosterUrl = m.Key.PosterUrl,
                        Title = m.Key.Title,
                        ReleaseDate = m.Key.ReleaseDate,
                        Rating = m.Average(x => x.Rating)
                    })
                    .Take(50)
                    .ToListAsync();

            return topRatedMovies;
        }

        public async Task<Movie> GetMovieReviews(int id)
        {
            var movie = await _dbContext.Movies.Include(m => m.Reviews).FirstOrDefaultAsync(m => m.Id == id);

            return movie;
        }
    }
}