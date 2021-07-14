using ApplicationCore.Entities;
using ApplicationCore.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    class MovieRepository : IMovieRepository

    {
        public List<Movie> GetHighestGrossingMovies()
        {
            throw new NotImplementedException();
        }
    }
}
