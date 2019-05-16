using MovInfo.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovInfo.Services
{
    public static class BusinessLogicValidatorMessages
    {
        public const string NoSuchUser = "No such User in app yet!";
        public const string NoSuchMovie = "No such Movie in app yet!";
        public const string NoSuchCategory = "No such Category in app yet!";
        public const string NoSuchActor = "No such Actor in app yet!";
        public const string NoSuchReview = "No such Review in app yet!";
        public const string NoReviewsFromUser = "User does not have any reviews yet!";
        public const string NoMoviesInApp = "No Movies in the app yet!!";

        public const string OneOrMoreActorsMissing = "One or more Actors do not exist!";
        public const string OneOrMoreCategoriesMissing = "One or more Categories do not exist!";

        public const string UserAlreadyPresent = "User already present!";
        public const string MovieAlreadyPresent = "Movie already present!";
        public const string CategoryAlreadyPresent = "Category already present!";
        public const string ActorAlreadyPresent = "Actor already present!";

        public const string NotAuthorized = "You Are not Authorized for that action!";
        public const string IdsDoNotExist = "Those Ids do not exist!";
    }
}
