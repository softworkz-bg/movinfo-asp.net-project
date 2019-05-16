using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovInfo.Web.ViewModels;

namespace MovInfo.Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult ShowErrorPage(string message)
        {
            var viewModel = new AppErrorViewModel() { ErrorMessage = message };

            return View("GeneralError", viewModel);
        }
    }
}