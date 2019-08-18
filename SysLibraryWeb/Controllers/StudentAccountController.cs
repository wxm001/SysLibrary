using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SysLibraryWeb.Controllers
{
    using Microsoft.AspNetCore.Authorization;

    using SysLibraryWeb.Models;
    [Authorize]
    public class StudentAccountController : Controller
    {
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated) //判断是否授权以避免多余的授权
            {
                return this.RedirectToAction("AccountInfo");
            }
            LoginViewModel loginInfo=new LoginViewModel();
            ViewBag.returnUrl = returnUrl;
            return View(loginInfo);
        }
    }
}