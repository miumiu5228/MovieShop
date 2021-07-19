using ApplicationCore.Entities;
using ApplicationCore.Models;
using ApplicationCore.RepositoryInterfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GenreRepository : EfRepository<Genre>, IGenreRepository
    {
        public GenreRepository(MovieShopDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<IEnumerable<GenreModel>> GetAllGenres()
        {
            List<Genre> Genres = await _dbContext.Genres.ToListAsync();
            return (IEnumerable<GenreModel>)Genres;
        }
    }
}
