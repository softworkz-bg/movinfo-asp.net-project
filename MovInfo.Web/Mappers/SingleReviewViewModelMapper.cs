using MovInfo.Models;
using MovInfo.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovInfo.Web.Mappers
{
    public class SingleReviewViewModelMapper : IViewModelMapper<Review, SingleReviewViewModel>
    {
        public SingleReviewViewModel MapFrom(Review entity)
        => new SingleReviewViewModel
        {
             Id = entity.Id,
             Rating = entity.Rating,
             Text = entity.Text,
             MovieId = entity.MovieId,
             ApplicationUserName = entity.ApplicationUserName,
             ApplicationUserId = entity.ApplicationUserId
        };
    }
}
