using ApplicationCore.Models;
using ApplicationCore.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class MovieService : IMovieService
    {
        public List<MovieCardResponseModel> GetTopRevenueMovies()
        {
            var movies = new List<MovieCardResponseModel> {

                          new MovieCardResponseModel {Id = 1, Title = "Avengers: Infinity War", Budget = 1200000},
                          new MovieCardResponseModel {Id = 2, Title = "Avatar", Budget = 1200000},
                          new MovieCardResponseModel {Id = 3, Title = "Star Wars: The Force Awakens", Budget = 1200000},
                          new MovieCardResponseModel {Id = 4, Title = "Titanic", Budget = 1200000},
                          new MovieCardResponseModel {Id = 5, Title = "Inception", Budget = 1200000},
                          new MovieCardResponseModel {Id = 6, Title = "Avengers: Age of Ultron", Budget = 1200000},
                          new MovieCardResponseModel {Id = 7, Title = "Interstellar", Budget = 1200000},
                          new MovieCardResponseModel {Id = 8, Title = "Fight Club", Budget = 1200000},
            };

            return movies;
        }

       
    }
}
