using Microsoft.Extensions.Configuration;
using MovInfo.Models;
using MovInfo.Web.ViewModels;

namespace MovInfo.Web.Mappers
{
    public class SingleActorViewModelMapper : IViewModelMapper<Actor, SingleActorViewModel>
    {
        private readonly IConfiguration configuration;

        public SingleActorViewModelMapper(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public SingleActorViewModel MapFrom(Actor entity)
        => new SingleActorViewModel
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Bio = entity.Bio,
            MainImageName = entity.ProfileImageName,
            FullImagePath = configuration.GetSection("DefaultImageFolder").Value + entity.ProfileImageName
        };
    }
}
