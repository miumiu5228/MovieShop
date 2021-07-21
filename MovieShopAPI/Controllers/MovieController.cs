using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }


        // attribute based routing
        [HttpGet]
        [Route("genre/{id:int}")]
        public async Task<IActionResult> GetMoviesByGenre(int id)
        {
            var movie = await _movieService.GetfilterGenres(id);
            
            if(movie == null)
            {
                return NotFound($"No Movie Found for that {id}");
            }

            return Ok(movie);
        }
        


        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetMovie(int id)
        {
            var movie = await _movieService.GetMovieDetails(id);

            if (movie == null)
            {
                return NotFound($"No Movie Found for that {id}");
            }
            return Ok(movie);
        }


        // attribute based routing
        [HttpGet]
        [Route("toprevenue")]
        public async Task<IActionResult> GetTopRevenueMovies()
        {
            var movies = await _movieService.GetTopRevenueMovies();

            if (!movies.Any())
            {
                return NotFound("No Movies Found");
            }

            return Ok(movies);

        }

        [HttpGet]
        [Route("toprated")]
        public async Task<IActionResult> GetTopRatingMovies()
        {
            var movies = await _movieService.GetTopRatingMovies();

            if (!movies.Any())
            {
                return NotFound("No Movie Found");
            }

            return Ok(movies);

        }

        [HttpGet]
        [Route("{id:int}/reviews")]
        public async Task<IActionResult> GetMovieReview(int id)
        {
            var movie = await _movieService.GetMovieReviews(id);

            if (movie == null)
            {
                return NotFound("No Movies Found");
            }

            return Ok(movie);

        }


    }
}
