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
    public class CastService : ICastService
    {
        private readonly ICastRepository _castRepository;

        public CastService(ICastRepository castRepository)
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


        public async Task<CastResponseModel> GetCastById(int id)
        {
            var x = 0;
            x += 1;
            var cast = await _castRepository.GetByIdAsync(id);

            var castDetails = new CastResponseModel()
            {
                Id = cast.Id,
                Name = cast.
                Gender = cast.Gender,
                TmdbUrl = cast.TmdbUrl,
                ProfilePath = cast.ProfilePath,
            };

            return castDetails;

        }
    }
}
