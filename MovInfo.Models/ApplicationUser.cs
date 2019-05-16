using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace MovInfo.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string ProfileImageName { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}
