using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MovInfo.ImageOptimizer;
using MovInfo.Models;
using MovInfo.Services.Contracts;
using MovInfo.Web.Mappers;
using MovInfo.Web.ViewModels;

namespace MovInfo.Web.Controllers
{
    public class ActorController : Controller
    {
        private readonly IMovieServices movieServices;
        private readonly IActorServices actorServices;
        private readonly IMovInfoImageOptimizer imageOptimizer;
        private readonly IConfiguration config;
        private readonly IViewModelMapper<Actor, SingleActorViewModel> actorMapper;
        private readonly IViewModelMapper<Movie, SingleMovieViewModel> movieMapper;

        public ActorController(IMovieServices movieServices,
            IActorServices actorServices,
            IMovInfoImageOptimizer imageOptimizer,
            IConfiguration config,
            IViewModelMapper<Actor, SingleActorViewModel> actorMapper,
            IViewModelMapper<Movie, SingleMovieViewModel> movieMapper)
        {
            this.movieServices = movieServices;
            this.actorServices = actorServices;
            this.imageOptimizer = imageOptimizer;
            this.config = config;
            this.actorMapper = actorMapper;
            this.movieMapper = movieMapper;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var actors = await this.actorServices.GetActorsByIdAsync();
                var actorViewModel = actors.Select(actorMapper.MapFrom).ToList();
                return View(actorViewModel);
            }
            catch (Exception)
            {
                return RedirectToAction("ShowErrorPage", "Error", new { message = "Something bad happened." });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Admin")]
        public IActionResult AddActor()
        {
            try
            {
                var actorFormViewModel = new SingleActorViewModel();

                return View("AddActor", actorFormViewModel);
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
        public async Task<ActionResult> FindActors(string firstName, string ignoredActors)
        {
            try
            {
                var resultedActors = await actorServices.FindActorsAsync(firstName, ignoredActors);

                var viewModel = new ActorViewModel
                {
                    AllActorIdsAndNames = resultedActors.ToDictionary(x => x.Id, y => y.LastName),

                    ActorIds = resultedActors.Select(x => x.Id).ToList()
                };

                return PartialView("_ActorsPartial", viewModel);
            }
            catch (Exception)
            {
                return RedirectToAction("ShowErrorPage", "Error", new { message = "Something bad happened." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> SaveSingleActor(SingleActorViewModel actorViewModelToBeSaved)
        {
            if (!ModelState.IsValid)
            {
                return View("AddActor", actorViewModelToBeSaved);
            }

            try
            {
                var allowedRoles = new string[] { "Admin", "Manager" };
                var finalImageName = await imageOptimizer.OptimizeImage(actorViewModelToBeSaved.MainImage, 300, 300);

                var addedActor = await actorServices.AddActorAsync(
                    actorViewModelToBeSaved.FirstName,
                    actorViewModelToBeSaved.LastName,
                    actorViewModelToBeSaved.Bio,
                    finalImageName,
                    allowedRoles);

                return RedirectToAction("GetActor", new { actorId = addedActor.Id });
            }
            catch (ArgumentException ex)
            {
                StatusMessage = ex.Message;
                return RedirectToAction("AddActor", actorViewModelToBeSaved);
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
        public async Task<IActionResult> GetActor(long actorId)
        {
            try
            {
                var actorInfo = await actorServices.ShowActorInfoAsync(actorId);
                var actorMovies = await actorServices.GetActorMoviesAsync(actorId);

                var mapedActor = actorMapper.MapFrom(actorInfo);
                mapedActor.AllMovies = actorMovies.Select(this.movieMapper.MapFrom).ToList();

                if (User.Identity.IsAuthenticated)
                {
                    if (User.FindFirst(ClaimTypes.Role).Value == "Manager" || User.FindFirst(ClaimTypes.Role).Value == "Admin")
                    {
                        mapedActor.CanUserEdit = true;
                        mapedActor.CanUserDelete = true;
                    }
                }

                return View("SingleActor", mapedActor);
            }
            catch (Exception)
            {
                return RedirectToAction("ShowErrorPage", "Error", new { message = "Something bad happened." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSingleActor(SingleActorViewModel actorViewModelToSave)
        {
            if (!ModelState.IsValid)
            {
                return View("EditActor", actorViewModelToSave);
            }

            try
            {
                var allowedRoles = new string[] { "Admin", "Manager" };

                var imageName = actorViewModelToSave.MainImageName;

                if (actorViewModelToSave.MainImage != null)
                {
                    var oldImage = imageName;
                    imageName = await imageOptimizer.OptimizeImage(actorViewModelToSave.MainImage, 300, 300);
                    imageOptimizer.DeleteOldImage(oldImage);
                }

                var editedActor = await actorServices.EditActorAsync(
                    actorViewModelToSave.Id,
                    actorViewModelToSave.FirstName,
                    actorViewModelToSave.LastName,
                    actorViewModelToSave.Bio,
                    imageName,
                    allowedRoles);

                return RedirectToAction("GetActor", new { actorId = editedActor.Id });
            }
            catch (ArgumentException ex)
            {
                StatusMessage = ex.Message;
                return View("EditActor", actorViewModelToSave);
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
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> EditActor(long actorId)
        {
            try
            {
                var actorWithAllInfo = await actorServices.ShowActorInfoAsync(actorId);
                var allMoviesForActor = await actorServices.GetActorMoviesAsync(actorId);

                var allMoviesIds = (allMoviesForActor.Select(x => x.Id).ToList());
                var allMoviesIdsString = string.Join(',', allMoviesIds);

                var mappedActor = actorMapper.MapFrom(actorWithAllInfo);
                mappedActor.AllMovies = allMoviesForActor.Select(this.movieMapper.MapFrom).ToList();

                return View(mappedActor);
            }
            catch (ArgumentException ex)
            {
                StatusMessage = ex.Message;
                return RedirectToAction("EditActor", new { actorId });
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
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> DeleteActor(long actorId)
        {
            try
            {
                var allowedRoles = new string[] { "Admin", "Manager" };

                var actor = await actorServices.DeleteActorAsync(actorId, allowedRoles);

                var actorViewModel = new SingleActorViewModel();

                var mappedActor = actorMapper.MapFrom(actor);

                return View("DeleteActor", actorViewModel);
            }
            catch (ArgumentException ex)
            {
                StatusMessage = ex.Message;
                return RedirectToAction("GetActor", new { actorId });
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