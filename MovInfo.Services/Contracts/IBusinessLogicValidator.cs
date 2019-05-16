using MovInfo.Data;
using MovInfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovInfo.Services.Contracts
{
    public interface IBusinessLogicValidator
    {
        void IsEntityFound(object entityToCheck, string messageToShow);

        void IsEntityAlreadyPresent(object entityToCheck, string messageToShow);

        void AreAnyEntitiesPresent(IQueryable<object> entitiesToCheck, string messageToShow);

        void DoesMovieExist(MovInfoContext context, long movieId);

        void DoesCategoryExist(MovInfoContext context, long categoryId);

        void DoesActorExist(MovInfoContext context, long actorId);

        void DoesReviewExist(MovInfoContext context, long reviewId);     

        void IsMovieAlreadyPresent(MovInfoContext context, string movieName, int movieYear);

        void IsAnotherMovieAlreadyPresent(MovInfoContext context, string movieName, int movieYear, long currMovieId);

        void IsCategoryAlreadyPresent(MovInfoContext context, string categoryName);

        void DoAllActorsExist(MovInfoContext context, IEnumerable<long> allMovieActors);

        void DoAllCategoriesExist(MovInfoContext context, IEnumerable<long> allMovieCategories);

        void IsActorAlreadyPresent(MovInfoContext context, string actorFirstName, string actorLastName);

        void IsUserInRoleForService(ICollection<string> rolesToCheckAgainstUser, string messageToShow);

        void IsAuthorOfEntity(string idOfEntityAuthor, string messageToShow);

        void IsUserAuthenticated(string messageToShow);

        void IsUserInRoleOrAuthor(ICollection<string> rolesToCheckAgainstUser, string authorIdOfTarget, string messageToShow);
    }
}
