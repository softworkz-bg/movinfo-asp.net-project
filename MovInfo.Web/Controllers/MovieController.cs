using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ImageMagick;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using MovInfo.Data;
using MovInfo.ImageOptimizer;
using MovInfo.Models;
using MovInfo.Services.Contracts;
using MovInfo.Web.Mappers;
using MovInfo.Web.ViewModels;

namespace MovInfo.Web.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieServices movieServices;
        private readonly IActorServices actorServices;
        private readonly ICategoryServices categoryServices;
        private readonly IMovInfoImageOptimizer imageOptimizer;
        private readonly IConfiguration config;
        private readonly IViewModelMapper<Actor, SingleActorViewModel> actorMapper;
        private readonly IViewModelMapper<Category, SingleCategoryViewModel> categoryMapper;
        private readonly IViewModelMapper<Review, SingleReviewViewModel> reviewMapper;
        private readonly IViewModelMapper<Movie, SingleMovieViewModel> movieMapper;
        private readonly IMemoryCache cache;

        public MovieController(IMovieServices movieServices,
            IMovInfoImageOptimizer imageOptimizer,
            IActorServices actorServices,
            ICategoryServices categoryServices,
            IConfiguration config,
            IViewModelMapper<Actor, SingleActorViewModel> actorMapper,
            IViewModelMapper<Category, SingleCategoryViewModel> categoryMapper,
            IViewModelMapper<Review, SingleReviewViewModel> reviewMapper,
            IViewModelMapper<Movie, SingleMovieViewModel> movieMapper,
            IMemoryCache cache)
        {
            this.movieServices = movieServices;
            this.imageOptimizer = imageOptimizer;
            this.actorServices = actorServices;
            this.categoryServices = categoryServices;
            this.config = config;
            this.actorMapper = actorMapper;
            this.categoryMapper = categoryMapper;
            this.reviewMapper = reviewMapper;
            this.movieMapper = movieMapper;
            this.cache = cache;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var movies = await this.movieServices.GetMoviesByRatingAsync();
                var movieViewModel = movies.Select(movieMapper.MapFrom).ToList();
                return View(movieViewModel);
            }
            catch (Exception)
            {
                return RedirectToAction("ShowErrorPage", "Error", new { message = "Something bad happened." });
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetMovie(long movieId)
        {
            try
            {
                var movieWithAllInfo = await movieServices.GetMovieWithAllInfoAsync(movieId);
                var allActorsForMovie = await movieServices.GetMovieActorsAsync(movieId);
                var allCategoriesForMovie = await movieServices.GetMovieCategoriesAsync(movieId);
                var topEightMovies = await movieServices.GetTopEightOrLessMoviesAsync();

                var mappedMovie = movieMapper.MapFrom(movieWithAllInfo);
                mappedMovie.AllActors = allActorsForMovie.Select(this.actorMapper.MapFrom).ToList();
                mappedMovie.AllCategories = allCategoriesForMovie.Select(this.categoryMapper.MapFrom).ToList();
                mappedMovie.AllReviews = movieWithAllInfo.Reviews.Select(this.reviewMapper.MapFrom).ToList();
                var allMappedTopEightMovies = topEightMovies.Select(this.movieMapper.MapFrom).ToList();

                mappedMovie.CurrentReview = new SingleReviewViewModel() { MovieId = mappedMovie.Id };

                if (User.Identity.IsAuthenticated)
                {
                    if (User.FindFirst(ClaimTypes.Role).Value == "Manager" || User.FindFirst(ClaimTypes.Role).Value == "Admin")
                    {
                        mappedMovie.CanUserEdit = true;
                    }

                    mappedMovie.CurrentReview.ApplicationUserName = User.FindFirst(ClaimTypes.Name).Value;
                    mappedMovie.CurrentReview.ApplicationUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                }

                var cachedTopEightMoviesInMemory = await cache.GetOrCreateAsync<IList<SingleMovieViewModel>>("TopEightMoviesForSingleMoviePage", async (cacheEntry) =>
                {
                    var cachedMovies = (allMappedTopEightMovies);
                    cacheEntry.SlidingExpiration = TimeSpan.FromDays(1);
                    return cachedMovies;
                });

                mappedMovie.TopEightMovies = new TopMoviesViewModel() { TopMovies = cachedTopEightMoviesInMemory };

                return View("SingleMovie", mappedMovie);
            }
            catch (Exception)
            {
                return RedirectToAction("ShowErrorPage", "Error", new { message = "Something bad happened." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListAllMoviesWithName(string name, int? targetPage)
        {
            try
            {
                var sixMoviesWithName = await movieServices.ListSixMoviesWithNameAsync(name, targetPage);
                var sixMappedMovies = sixMoviesWithName.Select(this.movieMapper.MapFrom).ToList();

                MovieListingViewModel viewModel = new MovieListingViewModel()
                {
                    SixMoviesWithName = sixMappedMovies,
                    SearchName = name,
                    TotalPagesCount = (sixMappedMovies.Count / 6) + 1
                };

                if (targetPage == null)
                {
                    viewModel.TargetPage = 1;
                }

                else
                {
                    viewModel.TargetPage = (int)targetPage;
                }

                return View("MovieListing", viewModel);
            }
            catch (Exception)
            {
                return RedirectToAction("ShowErrorPage", "Error", new { message = "Something bad happened." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GridAllMoviesWithName(string name, int? targetPage)
        {
            try
            {
                var sixMoviesWithName = await movieServices.ListSixMoviesWithNameAsync(name, targetPage);
                var sixMappedMovies = sixMoviesWithName.Select(this.movieMapper.MapFrom).ToList();

                MovieListingViewModel viewModel = new MovieListingViewModel()
                {
                    SixMoviesWithName = sixMappedMovies,
                    SearchName = name,
                    TotalPagesCount = (sixMappedMovies.Count / 6) + 1
                };

                if (targetPage == null)
                {
                    viewModel.TargetPage = 1;
                }

                else
                {
                    viewModel.TargetPage = (int)targetPage;
                }

                return View("MovieGrid", viewModel);
            }
            catch (Exception)
            {
                return RedirectToAction("ShowErrorPage", "Error", new { message = "Something bad happened." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTopEightMovies()
        {
            try
            {
                var topEightMovies = await movieServices.GetTopEightOrLessMoviesAsync();
                var allMappedMovies = topEightMovies.Select(this.movieMapper.MapFrom).ToList();

                var cachedMoviesInMemory = await cache.GetOrCreateAsync<IList<SingleMovieViewModel>>("TopEightMovies", async (cacheEntry) =>
                {
                    var cachedMovies = (allMappedMovies);
                    cacheEntry.SlidingExpiration = TimeSpan.FromDays(1);
                    return cachedMovies;
                });

                return View("SingleMovie", cachedMoviesInMemory);
            }
            catch (Exception)
            {
                return RedirectToAction("ShowErrorPage", "Error", new { message = "Something bad happened." });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Admin")]
        public IActionResult AddMovie()
        {
            try
            {
                var movieFormViewModel = new SingleMovieViewModel();

                return View("AddMovie", movieFormViewModel);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ShowErrorPage", "Error", new { message = ex.Message });
            }            
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> EditMovie(long movieId)
        {
            try
            {
                var movieWithAllInfo = await movieServices.GetMovieWithAllInfoAsync(movieId);
                var allActorsForMovie = await movieServices.GetMovieActorsAsync(movieId);
                var allCategoriesForMovie = await movieServices.GetMovieCategoriesAsync(movieId);
                var allActorsIds = (allActorsForMovie.Select(x => x.Id).ToList());
                var allActorsIdsString = string.Join(',', allActorsIds);
                var allCategoriesIds = (allCategoriesForMovie.Select(x => x.Id).ToList());
                var allCategoriesIdsString = string.Join(',', allCategoriesIds);

                var mappedMovie = movieMapper.MapFrom(movieWithAllInfo);
                mappedMovie.AllActors = allActorsForMovie.Select(this.actorMapper.MapFrom).ToList();
                mappedMovie.AllCategories = allCategoriesForMovie.Select(this.categoryMapper.MapFrom).ToList();
                mappedMovie.AllReviews = movieWithAllInfo.Reviews.Select(this.reviewMapper.MapFrom).ToList();

                mappedMovie.AllActorsIdsString = allActorsIdsString;
                mappedMovie.AllCategoriesIdsString = allCategoriesIdsString;

                return View(mappedMovie);
            }
            catch (ArgumentException ex)
            {
                StatusMessage = ex.Message;
                return RedirectToAction("EditMovie", new { movieId });
            }
            catch (UnauthorizedAccessException ex)
            {
                return RedirectToAction("ShowErrorPage", "Error", new { message = ex.Message });
            }
            catch (Exception)
            {
                return RedirectToAction("ShowErrorPage", "Error", new { message = "Something bad happened." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> SearchMoviesByName(char id)
        {
            try
            {
                var moviesByStartingSymbol = await movieServices.GetMoviesStartingWithAsync(id);

                var movieViewModel = moviesByStartingSymbol.Select(movieMapper.MapFrom).ToList();

                return View(movieViewModel);
            }
            catch (Exception)
            {
                return RedirectToAction("ShowErrorPage", "Error", new { message = "Something bad happened." });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMovie(long movieId)
        {
            try
            {
                var allowedRoles = new string[] { "Admin", "Manager" };

                var deletedMovie = await movieServices.DeleteMovieAsync(movieId, allowedRoles);
                var movieFormViewModel = new SingleMovieViewModel();
                var mappedDeletedMovie = movieMapper.MapFrom(deletedMovie);

                return View("MovieDeleted", movieFormViewModel);
            }
            catch (ArgumentException ex)
            {
                StatusMessage = ex.Message;
                return RedirectToAction("EditMovie", new { movieId });
            }
            catch (UnauthorizedAccessException ex)
            {
                return RedirectToAction("ShowErrorPage", "Error", new { message = ex.Message });
            }
            catch (Exception)
            {
                return RedirectToAction("ShowErrorPage", "Error", new { message = "Something bad happened." });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<ActionResult> FindMovies(string movieName, string ignoredMovies)
        {
            try
            {
                var resultedMovies = await movieServices.FindMoviesAsync(movieName, ignoredMovies);

                var viewModel = new MovieViewModel
                {
                    AllMoviesIdsAndNames = resultedMovies.ToDictionary(x => x.Id, y => y.Name),

                    MoviesIds = resultedMovies.Select(x => x.Id).ToList()
                };

                return PartialView("_MoviesPartial", viewModel);
            }
            catch (Exception)
            {
                return RedirectToAction("ShowErrorPage", "Error", new { message = "Something bad happened." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> UpdateSingleMovie(SingleMovieViewModel movieViewModelToBeSaved)
        {
            if (movieViewModelToBeSaved.AllActorsIdsString == null || movieViewModelToBeSaved.AllCategoriesIdsString == null)
            {
                StatusMessage = "Cannot leave a movie without actors!";
                return RedirectToAction("GetMovie", new { movieId = movieViewModelToBeSaved.Id });
            }

            if (!ModelState.IsValid)
            {
                return View("EditMovie", movieViewModelToBeSaved);
            }

            try
            {
                if (!movieViewModelToBeSaved.MainImage.ContentType.Contains("image"))
                {
                    StatusMessage = "You must upload a valid picture!";
                    return View("EditMovie", movieViewModelToBeSaved);
                }
                var allowedRoles = new string[] { "Admin", "Manager" };

                var allActorsIds = movieViewModelToBeSaved.AllActorsIdsString.Split(',').Select(x => long.Parse(x)).Distinct().ToList();
                var allCategoriesIds = movieViewModelToBeSaved.AllCategoriesIdsString.Split(',').Select(x => long.Parse(x)).Distinct().ToList();
                var finalImageName = movieViewModelToBeSaved.MainImageName;

                if (movieViewModelToBeSaved.MainImage != null)
                {
                    var oldImage = finalImageName;
                    finalImageName = await imageOptimizer.OptimizeImage(movieViewModelToBeSaved.MainImage, 444, 300);
                    imageOptimizer.DeleteOldImage(oldImage);
                }

                var editedMovie = await movieServices.EditMovieAsync(
                    movieViewModelToBeSaved.Id,
                    movieViewModelToBeSaved.Name,
                    movieViewModelToBeSaved.DateCreated,
                    allCategoriesIds,
                    allActorsIds,
                    movieViewModelToBeSaved.Trailer,
                    movieViewModelToBeSaved.Bio,
                    finalImageName,
                    allowedRoles);

                return RedirectToAction("GetMovie", new { movieId = editedMovie.Id });
            }
            catch (ArgumentException ex)
            {
                StatusMessage = ex.Message;
                return View("EditMovie", movieViewModelToBeSaved);
            }
            catch (UnauthorizedAccessException ex)
            {
                return RedirectToAction("ShowErrorPage", "Error", new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return RedirectToAction("ShowErrorPage", "Error", new { message = ex.Message });
            }
            catch (Exception)
            {
                return RedirectToAction("ShowErrorPage", "Error", new { message = "Something bad happened." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> SaveSingleMovie(SingleMovieViewModel movieViewModelToBeSaved)
        {
            if (movieViewModelToBeSaved.AllActorsIdsString == null || movieViewModelToBeSaved.AllCategoriesIdsString == null)
            {
                StatusMessage = "Cannot leave a movie without actors!";
                return RedirectToAction("GetMovie", new { movieId = movieViewModelToBeSaved.Id });
            }

            if (!ModelState.IsValid)
            {              
                return View("AddMovie", movieViewModelToBeSaved);
            }

            try
            {
                var allowedRoles = new string[] { "Admin", "Manager" };

                var allActorsIds = movieViewModelToBeSaved.AllActorsIdsString.Split(',').Select(x => long.Parse(x)).Distinct().ToList();
                var allCategoriesIds = movieViewModelToBeSaved.AllCategoriesIdsString.Split(',').Select(x => long.Parse(x)).Distinct().ToList();

                var finalImageName = await imageOptimizer.OptimizeImage(movieViewModelToBeSaved.MainImage, 444, 300);

                var addedMovie = await movieServices.AddMovieAsync(
                    movieViewModelToBeSaved.Name,
                    movieViewModelToBeSaved.DateCreated,
                    allCategoriesIds,
                    allActorsIds,
                    movieViewModelToBeSaved.Trailer,
                    movieViewModelToBeSaved.Bio,
                    finalImageName,
                    allowedRoles);

                return RedirectToAction("GetMovie", new { movieId = addedMovie.Id });
            }
            catch (ArgumentException ex)
            {
                StatusMessage = ex.Message;
                return RedirectToAction("AddMovie", movieViewModelToBeSaved);               
            }
            catch (UnauthorizedAccessException ex)
            {
                return RedirectToAction("ShowErrorPage", "Error", new { message = ex.Message });
            }
            catch (Exception)
            {
                return RedirectToAction("ShowErrorPage", "Error", new { message = "Something bad happened." });
            }
        }        
    }
}