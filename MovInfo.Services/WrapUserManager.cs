using Microsoft.AspNetCore.Identity;
using MovInfo.Models;
using MovInfo.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovInfo.Services
{
    public class WrapUserManager : IWrapUserManager
    {
        public WrapUserManager(UserManager<ApplicationUser> userManager)
        {
            this.WrappedUserManager = userManager;
        }

        public UserManager<ApplicationUser> WrappedUserManager { get; set; }
    }
}
