using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.RepositoryInterfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CastRepository : EfRepository<Cast>, ICastRepository
    {
        public CastRepository(MovieShopDbContext dbContext) : base(dbContext)
        {
        }
        public Task<List<Cast>> GetCastDetails()
        {
            throw new NotImplementedException();
        }

        public override async Task<Cast> GetByIdAsync(int id)
        {
            var x=0;
            x += 1;

            var Cast = await _dbContext.Casts.Include(c => c.MovieCasts).ThenInclude(c => c.Movie).FirstOrDefaultAsync(c => c.Id == id);
               

            if (Cast == null)
            {
                throw new Exception($"No Movie Found with {id}");
            }

           
            return Cast;
        }
    }
}
