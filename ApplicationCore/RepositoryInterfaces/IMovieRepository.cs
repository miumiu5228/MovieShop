using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entities;

namespace ApplicationCore.RepositoryInterfaces
{
    public interface IMovieRepository : IAsyncRepository<Movie>
    {
        Task<List<Movie>> GetHighest30GrossingMovies();
        Task<List<Movie>> GetMoviesByTopRating();
        Task<List<Movie>> GetMoviesByGenre(int id);
        Task<Movie> GetMovieReviews(int id);
        Task<List<Movie>> GetMoviesCast();
       

    }
}