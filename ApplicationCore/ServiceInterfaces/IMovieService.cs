using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Models;

namespace ApplicationCore.ServiceInterfaces
{
    public interface IMovieService
    {
        Task<List<MovieCardResponseModel>> GetTopRevenueMovies();
        Task<List<MovieCardResponseModel>> GetTopRatingMovies();
        Task<MovieDetailsResponseModel> GetMovieDetails(int id);
        Task<List<MovieReviewsModel>> GetMovieReviews(int id);
        Task<List<MovieCardResponseModel>> GetfilterGenres(int id);
        Task<List<MovieCardResponseModel>> GetMovieByCast(int id);
        Task<MovieCardResponseModel> CreateMovie(MovieCreateResquestModel movie);
        Task<MovieDetailsResponseModel> UpdateMovie(MovieUpdateRequestModel movie);

    }
}