using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovInfo.Models;
using MovInfo.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovInfo.Web.Mappers
{
    public class AppUserViewModelMapper : IViewModelMapper<ApplicationUser, AppUserViewModel>
    {
        private readonly IConfiguration configuration;
        private readonly IServiceProvider serviceProvider;


        public AppUserViewModelMapper(
            IConfiguration configuration, 
            IServiceProvider serviceProvider)
        {
            this.configuration = configuration;
            this.serviceProvider = serviceProvider;
        }

        public AppUserViewModel MapFrom(ApplicationUser entity)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            return new AppUserViewModel
            {
                Id = entity.Id,
                Username = entity.UserName,
                Email = entity.Email,
                UserRoles = userManager.GetRolesAsync(entity).Result
            };
        }             
    }
}

