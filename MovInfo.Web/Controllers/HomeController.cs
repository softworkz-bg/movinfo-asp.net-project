using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovInfo.Models;
using MovInfo.Services.Contracts;
using MovInfo.Web.Mappers;
using MovInfo.Web.ViewModels;

namespace MovInfo.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMovieServices movieServices;
        private readonly IViewModelMapper<Movie, SingleMovieViewModel> movieMapper;

        public HomeController(
            IMovieServices movieServices, 
            IViewModelMapper<Movie, SingleMovieViewModel> movieMapper)
        {
            this.movieServices = movieServices;
            this.movieMapper = movieMapper;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var topEightMovies = await movieServices.GetTopEightOrLessMoviesAsync();
                var allMappedMovies = topEightMovies.Select(this.movieMapper.MapFrom).ToList();

                var view = new HomeIndexViewModel()
                {
                    TopEightMovies = new TopMoviesViewModel()
                    {
                        TopMovies = allMappedMovies
                    }
                };

                return View(view);
            }
            catch (Exception)
            {
                return RedirectToAction("ShowErrorPage", "Error", new { message = "Something bad happened." });
            }

        }

        public async Task<IActionResult> Privacy()
        {
            return View();
        }

        public async Task<IActionResult> AboutContacts()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("ShowErrorPage", "Error", new { message = "Something bad happened." });
            }
        }
       
        public async Task<IActionResult> ModalAction(int Id)
        {            
            return PartialView("_LoginPartial");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
