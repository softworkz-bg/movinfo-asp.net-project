using MovInfo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovInfo.Services.Contracts
{
    public interface IApplicationUserServices
    {
        Task<IReadOnlyCollection<ApplicationUser>> GetAllUsersAsync();

        Task<ApplicationUser> DeleteSingleUserAsync(string userId);
    }
}
