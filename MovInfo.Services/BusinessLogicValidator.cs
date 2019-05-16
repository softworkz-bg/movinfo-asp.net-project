using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MovInfo.Data;
using MovInfo.Models;
using MovInfo.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace MovInfo.Services
{
    public class BusinessLogicValidator : IBusinessLogicValidator
    {
        private readonly IWrapUserManager userManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public BusinessLogicValidator(IHttpContextAccessor httpContextAccessor, IWrapUserManager userManager)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }

        public void IsUserAuthenticated(string messageToShow)
        {
            var isAuthenticated = httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

            if (isAuthenticated == false)
            {
                throw new UnauthorizedAccessException(messageToShow);
            }
        }       

        public void IsUserInRoleForService(ICollection<string> rolesToCheckAgainstUser, string messageToShow)
        {
            bool isAllowed = false;

            var userId = httpContextAccessor
                .HttpContext
                .User
                .FindFirst(ClaimTypes.NameIdentifier)
                .Value;

            var user = userManager.WrappedUserManager.Users.First(x => x.Id == userId);

            foreach (var item in rolesToCheckAgainstUser)
            {
                if (userManager.WrappedUserManager.IsInRoleAsync(user, item).Result)
                {
                    isAllowed = true;
                    break;                    
                }
            }
            if (isAllowed == false)
            {
                throw new UnauthorizedAccessException(messageToShow);
            }            
        }

        public void IsUserInRoleOrAuthor(ICollection<string> rolesToCheckAgainstUser, string authorIdOfTarget, string messageToShow)
        {
            bool isAllowed = false;

            var userId = httpContextAccessor
                .HttpContext
                .User
                .FindFirst(ClaimTypes.NameIdentifier)
                .Value;            

            if (authorIdOfTarget != userId)
            {
                var user = userManager.WrappedUserManager.Users.First(x => x.Id == userId);

                foreach (var item in rolesToCheckAgainstUser)
                {
                    if (userManager.WrappedUserManager.IsInRoleAsync(user, item).Result)
                    {
                        isAllowed = true;
                        break;
                    }
                }

                if (isAllowed == false)
                {
                    throw new UnauthorizedAccessException(messageToShow);
                }
            }           
        }

        public void IsAuthorOfEntity(string idOfEntityAuthor, string messageToShow)
        {
            var userId = httpContextAccessor
                .HttpContext
                .User
                .FindFirst(ClaimTypes.NameIdentifier)
                .Value;

            if (idOfEntityAuthor != userId)
            {
                throw new UnauthorizedAccessException(messageToShow);
            }                      
        }

        public void IsEntityFound(object entityToCheck, string messageToShow)
        {
            if (entityToCheck == null)
            {
                throw new ArgumentException(messageToShow);
            }
        }

        public void IsEntityAlreadyPresent(object entityToCheck, string messageToShow)
        {
            if (entityToCheck != null)
            {
                throw new ArgumentException(messageToShow);
            }
        }

        public void AreAnyEntitiesPresent(IQueryable<object> entitiesToCheck, string messageToShow)
        {
            if (entitiesToCheck.Count() == 0)
            {
                throw new ArgumentException(messageToShow);
            }
        }

        public void DoesMovieExist(MovInfoContext context, long movieId)
        {
            if (!context.Movies.Any(m => m.Id == movieId))
            {
                throw new ArgumentException(BusinessLogicValidatorMessages.NoSuchMovie);
            }
        }

        public void DoesCategoryExist(MovInfoContext context, long categoryId)
        {
            if (!context.Categories.Any(c => c.Id == categoryId))
            {
                throw new ArgumentException(BusinessLogicValidatorMessages.NoSuchCategory);
            }
        }

        public void DoesActorExist(MovInfoContext context, long actorId)
        {
            if (!context.Actors.Any(a => a.Id == actorId))
            {
                throw new ArgumentException(BusinessLogicValidatorMessages.NoSuchActor);
            }
        }

        public void DoesReviewExist(MovInfoContext context, long reviewId)
        {
            if (!context.Reviews.Any(r => r.Id == reviewId))
            {
                throw new ArgumentException(BusinessLogicValidatorMessages.NoSuchReview);
            }
        }

        public void DoAllActorsExist(MovInfoContext context, IEnumerable<long> allMovieActors)
        {
            var result = context.Actors.Select(x => x.Id).Intersect(allMovieActors).ToList();

            if (!result.SequenceEqual(allMovieActors))
            {
                throw new ArgumentException(BusinessLogicValidatorMessages.OneOrMoreActorsMissing);
            }
        }

        public void DoAllCategoriesExist(MovInfoContext context, IEnumerable<long> allMovieCategories)
        {
            var result = context.Categories.Select(x => x.Id).Intersect(allMovieCategories).ToList();

            if (!result.SequenceEqual(allMovieCategories))
            {
                throw new ArgumentException(BusinessLogicValidatorMessages.OneOrMoreCategoriesMissing);
            }
        }

        public void IsMovieAlreadyPresent(MovInfoContext context, string movieName, int movieYear)
        {
            if (context.Movies.Any(m => m.Name == movieName && m.DateCreated.Year == movieYear))
            {
                throw new ArgumentException(BusinessLogicValidatorMessages.MovieAlreadyPresent);
            }
        }

        public void IsAnotherMovieAlreadyPresent(MovInfoContext context, string movieName, int movieYear, long currMovieId)
        {
            if (context.Movies.Any(m => m.Name == movieName && m.DateCreated.Year == movieYear && m.Id != currMovieId))
            {
                throw new InvalidOperationException(BusinessLogicValidatorMessages.MovieAlreadyPresent);
            }
        }

        public void IsCategoryAlreadyPresent(MovInfoContext context, string categoryName)
        {
            if (context.Categories.Any(c => c.Title == categoryName))
            {
                throw new ArgumentException(BusinessLogicValidatorMessages.CategoryAlreadyPresent);
            }
        }

        public void IsActorAlreadyPresent(MovInfoContext context, string actorFirstName, string actorLastName)
        {
            {
                if (context.Actors.Any(a => a.FirstName == actorFirstName && a.LastName == actorLastName))
                {
                    throw new ArgumentException(BusinessLogicValidatorMessages.ActorAlreadyPresent);
                }
            }
        }
    }
}
