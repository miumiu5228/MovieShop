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

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        //[HttpPost]
        //[Route("{purchases")]
        //public async Task<IActionResult> PurchasesMovie(PurchaseMovieModel purchaseMovie)
        //{
        //    var movie = await _userService.Purchase(purchaseMovie);


        //    if (movie == null)
        //    {
        //        return NotFound("Purchase movie filed");
        //    }

        //    return Ok(movie);


        //}

       


        [HttpGet]
        [Route("{id:int}/purchases")]
        public async Task<IActionResult> GetUserPurchases(int id)
        {
            var userPurchases = await _userService.GetPurchaseById(id);


            if (!userPurchases.Any())
            {
                return NotFound("No user Found");
            }

            return Ok(userPurchases);


        }


        [HttpPost]
        [Route("{id:int}/favorites")]
        public async Task<IActionResult> UserFavorite(UserFavoriteMoviesModel requestModel)
        {
            var userFavorites = await _userService.UserFavorite(requestModel);


            if (userFavorites == null)
            {
                return NotFound("Adding Filed");
            }

            return Ok(userFavorites);


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
    }
}
