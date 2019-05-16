using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MovInfo.Models;
using MovInfo.Services.Contracts;
using MovInfo.Web.Mappers;
using MovInfo.Web.ViewModels;

namespace MovInfo.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryServices categoryServices;
        private readonly IConfiguration config;
        private readonly IViewModelMapper<Movie, SingleMovieViewModel> movieMapper;
        private readonly IViewModelMapper<Category, SingleCategoryViewModel> categoryMapper;

        public CategoryController(ICategoryServices categoryServices,
            IConfiguration config,
            IViewModelMapper<Movie, SingleMovieViewModel> movieMapper,
            IViewModelMapper<Category, SingleCategoryViewModel> categoryMapper)
        {
            this.categoryServices = categoryServices;
            this.config = config;
            this.movieMapper = movieMapper;
            this.categoryMapper = categoryMapper;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = await this.categoryServices.GetCategoriesByIdAsync();
                var categoryViewModel = categories.Select(categoryMapper.MapFrom).ToList();
                return View(categoryViewModel);
            }
            catch (Exception)
            {
                return RedirectToAction("ShowErrorPage", "Error", new { message = "Something bad happened." });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Admin")]
        public IActionResult AddCategory()
        {
            try
            {
                var categoryFormViewModel = new SingleCategoryViewModel();

                return View("AddCategory", categoryFormViewModel);
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
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> SaveSingleCategory(SingleCategoryViewModel categoryViewModelToBeSaved)
        {
            if (!ModelState.IsValid)
            {
                return View("AddCategory", categoryViewModelToBeSaved);
            }

            try
            {
                var allowedRoles = new string[] { "Admin", "Manager" };

                var addedCategory = await categoryServices.AddCategoryAsync(categoryViewModelToBeSaved.Title, allowedRoles);

                return RedirectToAction("GetCategory", new { categoryId = addedCategory.Id});
            }
            catch (ArgumentException ex)
            {
                StatusMessage = ex.Message;
                return RedirectToAction("AddCategory", categoryViewModelToBeSaved);
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
        public async Task<ActionResult> FindCategories(string categoryName, string ignoredCategories)
        {
            try
            {
                var resultedCategories = await categoryServices.FindCategoriesAsync(categoryName, ignoredCategories);

                var viewModel = new CategoryViewModel
                {
                    AllCategoryIdsAndNames = resultedCategories.ToDictionary(x => x.Id, y => y.Title),

                    AllCategoryIds = resultedCategories.Select(x => x.Id).ToList()
                };

                return PartialView("_CategoriesPartial", viewModel);
            }
            catch (Exception)
            {
                return RedirectToAction("ShowErrorPage", "Error", new { message = "Something bad happened." });
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetCategory(long categoryId)
        {
            try
            {
                var categoryInfo = await categoryServices.ShowCategoryInfoAsync(categoryId);
                var categoryMovies = await categoryServices.GetCategoryMoviesAsync(categoryId);

                var mapedCategory = categoryMapper.MapFrom(categoryInfo);
                mapedCategory.AllMovies = categoryMovies.Select(this.movieMapper.MapFrom).ToList();

                if (User.Identity.IsAuthenticated)
                {
                    if (User.FindFirst(ClaimTypes.Role).Value == "Manager" || User.FindFirst(ClaimTypes.Role).Value == "Admin")
                    {
                        mapedCategory.CanUserEdit = true;
                        mapedCategory.CanUserDelete = true;
                    }
                }

                return View("ShowCategory", mapedCategory);
            }
            catch (Exception)
            {
                return RedirectToAction("ShowErrorPage", "Error", new { message = "Something bad happened." });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> EditCategory(long categoryId)
        {
            try
            {
                var categoryWithAllInfo = await categoryServices.ShowCategoryInfoAsync(categoryId);
                var allMoviesForCategory = await categoryServices.GetCategoryMoviesAsync(categoryId);

                var allMoviesIds = (allMoviesForCategory.Select(x => x.Id).ToList());
                var allMoviesIdsString = string.Join(',', allMoviesIds);

                var mappedCategory = categoryMapper.MapFrom(categoryWithAllInfo);
                mappedCategory.AllMovies = allMoviesForCategory.Select(this.movieMapper.MapFrom).ToList();

                mappedCategory.AllMoviesIdsString = allMoviesIdsString;

                return View(mappedCategory);
            }
            catch (ArgumentException ex)
            {
                StatusMessage = ex.Message;
                return RedirectToAction("EditCategory", new { categoryId });
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSingleCategory(SingleCategoryViewModel categoryViewModelToSave)
        {
            if (!ModelState.IsValid)
            {
                return View("EditCategory", categoryViewModelToSave);
            }

            try
            {
                var allowedRoles = new string[] { "Admin", "Manager" };

                var allMoviesIds = categoryViewModelToSave.AllMoviesIdsString.Split(',').Select(x => long.Parse(x)).Distinct().ToList();

                var editedCategory = await categoryServices.EditCategoryAsync(
                    categoryViewModelToSave.Id,
                    categoryViewModelToSave.Title,
                    allMoviesIds,
                    allowedRoles);

                return RedirectToAction("GetCategory", new { categoryId = editedCategory.Id });
            }
            catch (ArgumentException ex)
            {
                StatusMessage = ex.Message;
                return View("EditCategory", categoryViewModelToSave);
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(long categoryId)
        {
            try
            {
                var allowedRoles = new string[] { "Admin", "Manager" };

                var category = await categoryServices.DeleteCategoryAsync(categoryId, allowedRoles);

                var categoryViewModel = new SingleCategoryViewModel();

                var mappedCategory = categoryMapper.MapFrom(category);

                return View("DeleteCategory", categoryViewModel);
            }
            catch (ArgumentException ex)
            {
                StatusMessage = ex.Message;
                return RedirectToAction("GetCategory", new { categoryId });
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