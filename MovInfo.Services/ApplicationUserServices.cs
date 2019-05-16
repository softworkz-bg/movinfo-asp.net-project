using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovInfo.Data;
using MovInfo.Models;
using MovInfo.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovInfo.Services
{
    public class ApplicationUserServices : IApplicationUserServices
    {
        private readonly MovInfoContext context;
        private readonly IBusinessLogicValidator businessLogicValidator;
        private readonly IServiceProvider serviceProvider;
        private readonly IWrapUserManager wrapUserManager;

        public ApplicationUserServices
            (MovInfoContext context, 
            IBusinessLogicValidator businessLogicValidator,
            IServiceProvider serviceProvider,
            IWrapUserManager wrapUserManager)
        {
            this.context = context;
            this.businessLogicValidator = businessLogicValidator;
            this.serviceProvider = serviceProvider;
            this.wrapUserManager = wrapUserManager;
        }

        public async Task<IReadOnlyCollection<ApplicationUser>> GetAllUsersAsync()
        {
            var allUsers = await wrapUserManager.WrappedUserManager.Users.ToListAsync();

            return allUsers;
        }

        public async Task<ApplicationUser> DeleteSingleUserAsync(string userId)
        {
            var userToDelete = await wrapUserManager.WrappedUserManager.FindByIdAsync(userId);            

            var identityDeleted = await wrapUserManager.WrappedUserManager.DeleteAsync(userToDelete);

            await context.SaveChangesAsync();

            return userToDelete;
        }
    }
}
