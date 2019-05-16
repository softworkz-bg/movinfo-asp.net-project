using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovInfo.Models;
using MovInfo.Services.Contracts;
using MovInfo.Web.Mappers;
using MovInfo.Web.ViewModels;

namespace MovInfo.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IApplicationUserServices userServices;
        private readonly IViewModelMapper<ApplicationUser, AppUserViewModel> userMapper;

        public UsersController(
            IApplicationUserServices userServices,
            IViewModelMapper<ApplicationUser, AppUserViewModel> userMapper)
        {            
            this.userServices = userServices;
            this.userMapper = userMapper;
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUsersTable()
        {
            try
            {
                var allAppUsers = await userServices.GetAllUsersAsync();
                var allUsersViewModel = allAppUsers.Select(userMapper.MapFrom).ToList();
                var viewModel = new AllAppUsersViewModel { AllUsers = allUsersViewModel };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ShowErrorPage", "Error", new { message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSingleUser(string id)
        {
            try
            {
                var deletedUser = await userServices.DeleteSingleUserAsync(id);
                var viewModel = userMapper.MapFrom(deletedUser);

                return View("UserDeleted", viewModel);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ShowErrorPage", "Error", new { message = ex.Message });
            }
        }
    }
}