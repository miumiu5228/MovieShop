using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.RepositoryInterfaces;
using ApplicationCore.ServiceInterfaces;

namespace Infrastructure.Services
{
    public class CastsService : ICastService
    {
        private readonly ICastRepository _castRepository;

        public CastsService(ICastRepository castRepository)
        {
            _castRepository = castRepository;
        }
        public async Task<CastResponseModel> GetMovieByCast(int id)
        {
            var cast = await _castRepository.GetByIdAsync(id);

            var castDetails = new CastResponseModel()
            {
                Id = cast.Id,
                Name = cast.Name,
                Gender = cast.Gender,
                TmdbUrl = cast.TmdbUrl,
                ProfilePath = cast.ProfilePath,
            };

            castDetails.MovieCardResponseModel = new List<MovieCardResponseModel>();



            //var movieCards = new List<MovieCardResponseModel>();

            //foreach (var movie in movies)
            //{
            //    movieCards.Add(new MovieCardResponseModel { Id = movie.Id, Budget = movie.Budget.GetValueOrDefault(), PosterUrl = movie.PosterUrl, Title = movie.Title });
            //}

            //return movieCards;



            foreach (var movieCast in cast.MovieCasts)
            {
                castDetails.Character = movieCast.Character;
                

                castDetails.MovieCardResponseModel.Add(
                    new MovieCardResponseModel
                    {
                        Id = movieCast.MovieId,
                        Title = movieCast.Movie.Title,
                        Budget = movieCast.Movie.Budget.GetValueOrDefault(),
                        PosterUrl = movieCast.Movie.PosterUrl
                    }
                    );
                
            }

            return castDetails;
        }
    }
}
