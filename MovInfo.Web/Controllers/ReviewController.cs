using System;
using System.Collections.Generic;
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
    public class ReviewController : Controller
    {
        private readonly IReviewServices reviewServices;
        private readonly IActorServices actorServices;
        private readonly ICategoryServices categoryServices;
        private readonly IConfiguration config;
        private readonly IViewModelMapper<Review, SingleReviewViewModel> reviewMapper;

        public ReviewController(
            IReviewServices reviewServices,
            IActorServices actorServices,
            ICategoryServices categoryServices,
            IConfiguration config,
            IViewModelMapper<Review, SingleReviewViewModel> reviewMapper)
        {
            this.reviewServices = reviewServices;
            this.actorServices = actorServices;
            this.categoryServices = categoryServices;
            this.config = config;
            this.reviewMapper = reviewMapper;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        public async Task<IActionResult> GetReview(long id)
        {
            try
            {
                var reviewToShow = await reviewServices.GetReviewAsync(id);
                var mappedReview = reviewMapper.MapFrom(reviewToShow);

                if (User.Identity.IsAuthenticated)
                {
                    if (User.FindFirst(ClaimTypes.NameIdentifier).Value == mappedReview.ApplicationUserId)
                    {
                        mappedReview.CanUserEdit = true;
                    }
                }

                if (User.Identity.IsAuthenticated && (User.IsInRole("Admin") || User.IsInRole("Manager")))
                {
                    if (User.FindFirst(ClaimTypes.NameIdentifier).Value == mappedReview.ApplicationUserId)
                    {
                        mappedReview.CanUserDelete = true;
                    }
                }

                return View("SingleReview", mappedReview);
            }
            catch (Exception)
            {
                return RedirectToAction("ShowErrorPage", "Error", new { message = "Something bad happened." });
            }

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReview(long id)
        {
            try
            {
                var allowedRoles = new string[] { "Admin", "Manager" };

                var deletedReview = await reviewServices.DeleteReviewAsync(id, allowedRoles);

                return RedirectToAction("GetMovie", "Movie", new { movieId = deletedReview.MovieId });
            }
            catch (ArgumentException ex)
            {
                StatusMessage = ex.Message;
                return RedirectToAction("GetReview", new { id });
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
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveReviewAsync(SingleReviewViewModel reviewViewModel)
        {          
            if (!ModelState.IsValid)
            {               
                return View("AddReview", reviewViewModel);
            }

            try
            {
                var allowedRoles = new string[] { "Admin", "Manager" };

                var reviewText = reviewViewModel.Text;
                var reviewRating = reviewViewModel.Rating;
                var reviewAuthorName = reviewViewModel.ApplicationUserName;
                var reviewAuthorId = reviewViewModel.ApplicationUserId;
                var targetMovieId = reviewViewModel.MovieId;

                var resultedReview = await reviewServices.AddReviewAsync(
                    reviewAuthorName,
                    reviewAuthorId,
                    targetMovieId,
                    reviewRating,
                    reviewText,
                    allowedRoles);

                return RedirectToAction("GetMovie", "Movie", new { movieId = resultedReview.MovieId });
            }
            catch (ArgumentException ex)
            {
                StatusMessage = ex.Message;
                return RedirectToAction("GetMovie", "Movie", new { movieId = reviewViewModel.MovieId });
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
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateReviewAsync(SingleReviewViewModel reviewViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("AddReview", reviewViewModel);
            }

            try
            {
                var allowedRoles = new string[] { "Admin", "Manager" };

                var reviewId = reviewViewModel.Id;
                var reviewText = reviewViewModel.Text;
                var reviewRating = reviewViewModel.Rating;
                var reviewAuthorName = reviewViewModel.ApplicationUserName;
                var reviewAuthorId = reviewViewModel.ApplicationUserId;
                var targetMovieId = reviewViewModel.MovieId;
                var oldReviewRating = reviewViewModel.OldRating;

                var updatedReview = await reviewServices.UpdateReviewAsync(
                    reviewId,
                    reviewAuthorName,
                    reviewAuthorId,
                    targetMovieId,
                    reviewRating,
                    oldReviewRating,
                    reviewText,
                    allowedRoles);

                return RedirectToAction("GetReview", "Review", new { id = updatedReview.Id });
            }
            catch (ArgumentException ex)
            {
                StatusMessage = ex.Message;
                return RedirectToAction("GetReview", new { id = reviewViewModel.Id });
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