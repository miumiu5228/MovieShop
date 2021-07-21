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
    public class PurchaseRepository : EfRepository<Purchase>, IPurchaseRepository
    {
        public PurchaseRepository(MovieShopDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<List<Purchase>> GetPurchase(int id)
        {
            var purchases = await _dbContext.Purchases.Include(p => p.User).Include(p => p.Movie).Where(p=>p.UserId == id).ToListAsync();

            return purchases;  
        }
    }
}
