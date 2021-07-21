using ApplicationCore.Models;
using ApplicationCore.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewService _reviewService;

        public ReviewService(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        public async Task<List<MovieReviewsModel>> GetReviewsByUserId(int id)
        {
            var reviews = await _reviewService.GetReviewsByUserId(id);

            var userReview = new List<MovieReviewsModel>();
            foreach (var r in reviews)
            {
                userReview.Add(new MovieReviewsModel
                {
                    MovieName = r.MovieName,
                    ReviewText = r.ReviewText,
                    Rating = r.Rating,
                    UserId = r.UserId
                });


            }

            return userReview;
        }
    }
}
