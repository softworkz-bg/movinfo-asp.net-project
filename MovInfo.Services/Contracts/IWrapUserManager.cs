using Microsoft.AspNetCore.Identity;
using MovInfo.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovInfo.Services.Contracts
{
    public interface IWrapUserManager
    {
        UserManager<ApplicationUser> WrappedUserManager { get; set; }        
    }
}
