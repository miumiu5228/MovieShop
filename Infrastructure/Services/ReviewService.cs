using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Models;
using ApplicationCore.RepositoryInterfaces;
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
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        //public async Task<List<MovieReviewsModel>> GetReviewsByUserId(int id)
        //{
        //    var reviews = await _reviewService.GetReviewsByUserId(id);

        //    var userReview = new List<MovieReviewsModel>();
        //    foreach (var r in reviews)
        //    {
        //        userReview.Add(new MovieReviewsModel
        //        {
        //            MovieName = r.MovieName,
        //            ReviewText = r.ReviewText,
        //            Rating = r.Rating,
        //            UserId = r.UserId
        //        });


        //    }

        //    return userReview;
        //}

        public async Task<PostReviewsResponseModel> PostReviews(PostReviewsRequestModel model)
        {
            var dbReviews = await _reviewRepository.GetExistsAsync(f => f.MovieId == model.MovieId && f.UserId == model.UserId);

            if (dbReviews != false)
            {
                throw new ConflictException("You already posted a review for this movie");
            }
            await _reviewRepository.AddAsync(new Review
            {
                MovieId = model.MovieId,
                UserId = model.UserId,
                Rating = model.Rating,
                ReviewText = model.ReviewText
            });

            var postReviewsResponse = new PostReviewsResponseModel
            {
                MovieId = model.MovieId,
                UserId = model.UserId,
                Rating = model.Rating,
                ReviewText = model.ReviewText

            };

            return postReviewsResponse;
        }
    }
  }

