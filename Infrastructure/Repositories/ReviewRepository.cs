using ApplicationCore.Entities;
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
    public class ReviewRepository : EfRepository<Review>, IReviewRepository
    {
        public ReviewRepository(MovieShopDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<Review> GetReviews(int id, int movieId)
        {
            var review = await _dbContext.Reviews.FirstOrDefaultAsync(r=>r.MovieId == movieId && r.UserId == id);
            return review;



        }

        public async Task<List<Review>> GetReviewsByUser(int id)
        {
            var reviews = await _dbContext.Reviews.Include(r => r.UserId == id).ToListAsync();
            return reviews;
        }


    }




}
