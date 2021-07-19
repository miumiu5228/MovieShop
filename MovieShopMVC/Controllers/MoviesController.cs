using System.Threading.Tasks;
using ApplicationCore.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace MovieShopMVC.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;
      

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

      
        public async Task<IActionResult> Details(int id)
        {
            var movie = await _movieService.GetMovieDetails(id);
            return View(movie);
        }

        public async Task<IActionResult> Genre(int id)
        {

            var movie = await _movieService.GetfilterGenres(id);
            return View(movie);
        }

        public async Task<IActionResult> Cast(int id)
        {

            var movie = await _movieService.GetMovieByCast(id);
            return View(movie);
        }


    }
}