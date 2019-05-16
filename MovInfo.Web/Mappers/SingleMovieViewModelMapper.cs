using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MovInfo.Models;
using MovInfo.Web.ViewModels;

namespace MovInfo.Web.Mappers
{
    public class SingleMovieViewModelMapper : IViewModelMapper<Movie, SingleMovieViewModel>
    {
        private readonly IConfiguration configuration;

        public SingleMovieViewModelMapper(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public SingleMovieViewModel MapFrom(Movie entity)
             => new SingleMovieViewModel
             {
                 Id = entity.Id,
                 Name = entity.Name,
                 DateCreated = entity.DateCreated,
                 Rating = entity.Rating,
                 Trailer = entity.Trailer,
                 Bio = entity.Bio,
                 NumberOfRatings = entity.TotalRatings,
                 MainImageName = entity.MainImageName,
                 FullImagePath = configuration.GetSection("DefaultImageFolder").Value + entity.MainImageName
             };
    }
}