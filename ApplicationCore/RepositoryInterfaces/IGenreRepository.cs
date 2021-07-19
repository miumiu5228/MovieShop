using ApplicationCore.Entities;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.RepositoryInterfaces
{
    public interface IGenreRepository : IAsyncRepository<Genre>
    {
        Task<IEnumerable<GenreModel>> GetAllGenres();
    }
}
