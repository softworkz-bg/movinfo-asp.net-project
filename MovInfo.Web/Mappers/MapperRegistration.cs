using Microsoft.Extensions.DependencyInjection;
using MovInfo.Models;
using MovInfo.Web.ViewModels;

namespace MovInfo.Web.Mappers
{
    public static class MapperRegistration
    {
        public static IServiceCollection AddCustomMappers(this IServiceCollection services)
        {
            services.AddSingleton<IViewModelMapper<Movie, SingleMovieViewModel>, SingleMovieViewModelMapper>();
            services.AddSingleton<IViewModelMapper<Actor, SingleActorViewModel>, SingleActorViewModelMapper>();
            services.AddSingleton<IViewModelMapper<Review, SingleReviewViewModel>, SingleReviewViewModelMapper>();
            services.AddSingleton<IViewModelMapper<Category, SingleCategoryViewModel>, SingleCategoryViewModelMapper>();
            services.AddScoped<IViewModelMapper<ApplicationUser, AppUserViewModel>, AppUserViewModelMapper>();

            return services;
        }
    }
}
