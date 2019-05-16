using MovInfo.Data;
using MovInfo.Models;
using MovInfo.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovInfo.Services
{
    public class ReviewServices : IReviewServices
    {
        private readonly MovInfoContext context;
        private readonly IBusinessLogicValidator businessLogicValidator;

        public ReviewServices(MovInfoContext context, IBusinessLogicValidator businessLogicValidator)
        {
            this.context = context;
            this.businessLogicValidator = businessLogicValidator;
        }

        public async Task<Review> AddReviewAsync(
            string userName, 
            string userId,
            long movieId, 
            double reviewRating, 
            string reviewText,
            ICollection<string> allowedRoles)
        {
            businessLogicValidator.DoesMovieExist(context, movieId);
            businessLogicValidator.IsUserAuthenticated(BusinessLogicValidatorMessages.NotAuthorized);

            var movieToAddReviewFor = await context.Movies.FindAsync(movieId);

            businessLogicValidator.IsEntityFound(movieToAddReviewFor,
                BusinessLogicValidatorMessages.NoSuchMovie);

            var reviewToAdd = new Review
            {
                Rating = reviewRating,
                Text = reviewText,
                ApplicationUserId = userId,
                ApplicationUserName = userName,
                MovieId = movieId
            };

            context.Reviews.Add(reviewToAdd);

            //RecalculateRatingForMovie
            movieToAddReviewFor.TotalRatings++;
            movieToAddReviewFor.AllRatingsSum += reviewRating;
            movieToAddReviewFor.Rating = movieToAddReviewFor.AllRatingsSum / movieToAddReviewFor.TotalRatings;

            await context.SaveChangesAsync();

            return reviewToAdd;
        }

        public async Task<Review> UpdateReviewAsync(
            long reviewId, 
            string userName, 
            string userId, 
            long movieId, 
            double reviewRating,
            double oldRating, 
            string reviewText,
            ICollection<string> allowedRoles)
        {
            businessLogicValidator.DoesMovieExist(context, movieId);
            businessLogicValidator.DoesReviewExist(context, reviewId);
          
            var movieToEditReviewFor = await context.Movies.FindAsync(movieId);      
            var reviewToUpdate = await context.Reviews.FindAsync(reviewId);
            businessLogicValidator.IsUserInRoleOrAuthor(allowedRoles, reviewToUpdate.ApplicationUserId, BusinessLogicValidatorMessages.NotAuthorized);

            businessLogicValidator.IsEntityFound(movieToEditReviewFor,
               BusinessLogicValidatorMessages.NoSuchMovie);

            businessLogicValidator.IsEntityFound(reviewToUpdate,
                BusinessLogicValidatorMessages.NoSuchReview);

            reviewToUpdate.Rating = reviewRating;
            reviewToUpdate.Text = reviewText;

            //RecalculateRatingForMovie
            movieToEditReviewFor.AllRatingsSum -= oldRating;
            movieToEditReviewFor.AllRatingsSum += reviewRating;
            movieToEditReviewFor.Rating = movieToEditReviewFor.AllRatingsSum / movieToEditReviewFor.TotalRatings;

            await context.SaveChangesAsync();

            return reviewToUpdate;
        }

        public async Task<Review> GetReviewAsync(long reviewId)
        {
            var reviewToFind = await context.Reviews.FindAsync(reviewId);

            businessLogicValidator.IsEntityFound(reviewToFind,
               BusinessLogicValidatorMessages.NoSuchReview);

            return reviewToFind;
        }

        public async Task<Review> DeleteReviewAsync(long reviewId, ICollection<string> allowedRoles)
        {
            businessLogicValidator.DoesReviewExist(context, reviewId);

            var reviewToDelete = await context.Reviews.FindAsync(reviewId);

            businessLogicValidator.IsUserInRoleOrAuthor(allowedRoles, reviewToDelete.ApplicationUserId, BusinessLogicValidatorMessages.NotAuthorized);

            context.Reviews.Remove(reviewToDelete);

            await context.SaveChangesAsync();

            return reviewToDelete;
        }
    }
}
