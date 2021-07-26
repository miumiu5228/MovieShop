using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.ServiceInterfaces;

namespace MovieShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
       
        private readonly IUserService _userService;
        private readonly IReviewService _reviewService;
      

        public UserController(IUserService userService, IReviewService reviewService)
        {
            _userService = userService;
            _reviewService = reviewService;
        }

        [HttpPost]
        [Route("purchase")]
        public async Task<IActionResult> PurchasesMovie([FromBody] PurchaseMovieModel purchaseMovie)
        {

            var movie = await _userService.PurchaseMovie(purchaseMovie);


            if (movie == null)
            {
                return NotFound("Purchase movie filed");
            }

            return Ok(movie);


        }


        [HttpGet]
        [Route("{id:int}/purchases")]
        public async Task<IActionResult> GetUserPurchases(int id)
        {
            var x = 0;
            var userPurchases = await _userService.GetPurchaseById(id);


            if (!userPurchases.Any())
            {
                return NotFound("No movie Found");
            }

            return Ok(userPurchases);


        }


        [HttpPost]
        [Route("favorite")]
        public async Task<IActionResult> Favorite([FromBody] FavoriteRequestModel model)
        {
            var favoriteMovieResponse = await _userService.AddToFavorite(model);
            return Ok(favoriteMovieResponse);
        }

        [HttpPost]
        [Route("unfavorite")]
        public async Task<IActionResult> UnFavorite([FromBody] UnFavoriteRequestModel model)
        {
            var unfavoriteMovieResponse = await _userService.removefromFavorite(model);
            return Ok(unfavoriteMovieResponse);
        }




        [HttpGet]
        [Route("{id:int}/favorites")]
        public async Task<IActionResult> GetUserfavorites(int id)
        {
            var userFavorites = await _userService.GetFavoriteById(id);


            if (!userFavorites.Any())
            {
                return NotFound("No user Found");
            }

            return Ok(userFavorites);


        }

        [HttpGet]
        [Route("{id:int}/movie/{movieId:int}/favorite")]
        public async Task<IActionResult> GetFavoriteMovie(int id, int movieId)
        {
            var x = 0;
            var userPurchases = await _userService.GetFavoriteMovieDetail(id, movieId);


            if (userPurchases == null)
            {
                return NotFound("No movie Found");
            }

            return Ok(userPurchases);


        }



        [HttpGet]
        [Route("{id:int}/reviews")]
        public async Task<IActionResult> GetUserReview(int id)
        {
            var userReviews = await _userService.GetReviews(id);


            if (!userReviews.Any())
            {
                return NotFound("No user Found");
            }

            return Ok(userReviews);


        }


        [HttpPost]
        [Route("review")]
        public async Task<IActionResult> PostReview([FromBody] PostReviewsRequestModel model)
        {
           
            var createdReviews = await _userService.PostReviews(model);
             return Ok(createdReviews);
        }

        [HttpPut]
        [Route("review")]
        public async Task<IActionResult> PutReviews([FromBody] PostReviewsRequestModel model)
        {

            var createdReviews = await _userService.PutReviews(model);
            return Ok(createdReviews);
        }

        [HttpPut]
        [Route("{id:int}/movie/{movieId:int}")]
        public async Task<IActionResult> DeleteReviews(int id, int movieId)
        {
           
            var status = await _userService.DeleteReviews(id, movieId);
            return Ok(status);
        }

    }
}
