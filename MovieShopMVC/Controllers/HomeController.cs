using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieShopMVC.Models;
using Infrastructure.Services;
using ApplicationCore.ServiceInterfaces;

namespace MovieShopMVC.Controllers
{
    public class HomeController : Controller
    {
    

        private readonly IMovieService _movieService;

        public HomeController(IMovieService movieService)
        {
            _movieService = movieService;

        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
            var movies = await _movieService.GetTopRevenueMovies();
            return View(movies);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}