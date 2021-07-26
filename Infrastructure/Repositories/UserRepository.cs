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
    public class UserRepository : EfRepository<User>, IUserRepository
    {
        public UserRepository(MovieShopDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<User> GetFavorites(int id)
        {
            var user = await _dbContext.Users.Include(u=>u.Favorites).ThenInclude(u=>u.Movie).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        //public async Task<User> PurchaseMovie(int movieId,int userId)
        //{
        //    var user = await _dbContext.Users.Include(u => u.Purchases).FirstOrDefaultAsync(u => u.Id == userId);
        //    return user;
        //}

        public async Task<User> GetReviewsById(int id)
        {
            var user = await _dbContext.Users.Include(u => u.Reviews).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task<User> GetPurchase(int userId)
        {
            var user = await _dbContext.Users.Include(u => u.Purchases).FirstOrDefaultAsync(u => u.Id == userId);
            return user;
        }
    }

}
