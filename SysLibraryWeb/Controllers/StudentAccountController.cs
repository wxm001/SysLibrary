using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SysLibraryWeb.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;

    using SysLibraryWeb.Models;

    [Authorize] //确保已授权的用户才能访问对应动作方法
    public class StudentAccountController : Controller
    {
        private UserManager<Student> UserManager;

        private SignInManager<Student> SignInManager;
        //依赖注入
        public StudentAccountController(UserManager<Student> userManager,SignInManager<Student> signInManager)
        {
            this.UserManager = userManager;
            this.SignInManager = signInManager;
        }

        //登录界面
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

        //登录验证
        [HttpPost]
        [ValidateAntiForgeryToken] //防止XSRF攻击,
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginInfo, string returnUrl)//returnUrl用于接收和返回之前正在访问的界面,实现登录之后跳转到原来要访问的页面
        {
            if (ModelState.IsValid) //模型绑定之后需要检查一下
            {
                Student student = await GetStudentByLoginViewModel(loginInfo);
                if (student==null)
                {
                    return this.View(loginInfo);
                }

                SignInResult signInResult = 
                    await this.SignInManager.PasswordSignInAsync(student,loginInfo.Password,false,false);
                if (signInResult.Succeeded)
                {
                    return Redirect(returnUrl ?? "/StudentAccount/" + nameof(AccountInfo));
                }
            }

            return this.View(loginInfo);
        }

        //根据登录页面获取账户信息
        async Task<Student> GetStudentByLoginViewModel(LoginViewModel loginInfo)
        {
            Student student=new Student();
            switch (loginInfo.LoginType)
            {
                case LoginType.UserName:
                    student = await this.UserManager.FindByNameAsync(loginInfo.Account);
                    break;
                case LoginType.Email:
                    student = await this.UserManager.FindByEmailAsync(loginInfo.Account);
                    break;
                case LoginType.Phone:
                    student = this.UserManager.Users.First(s => s.PhoneNumber == loginInfo.Account);
                    break;
                default:
                    student = null;
                    break;
            }

            return student;
        }

        //登录成功后显示账户信息
        public IActionResult AccountInfo()
        {
            return this.View(CurrentAccountData());
        }

        //获取当前用户信息在AccountInfo视图呈现
        Dictionary<string, object> CurrentAccountData()
        {
            var userName = HttpContext.User.Identity.Name;
            var user = this.UserManager.FindByNameAsync(userName).Result;

            return new Dictionary<string, object>()
                       {
                           ["学号"]=userName,
                           ["姓名"]=user.Name,
                           ["邮箱"]=user.Email,
                           ["手机号"]=user.PhoneNumber
                       };
        }

        //登出
        public async Task<IActionResult> Logout(string returnUrl)
        {
            await this.SignInManager.SignOutAsync();
            if (returnUrl==null)
            {
                return this.View("Login");
            }
            return Redirect(returnUrl);
        }

        //修改密码视图
        public IActionResult ModifyPassword()
        {
            ModifyViewModel model=new ModifyViewModel();
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ModifyPassword(ModifyViewModel model)
        {
            if (ModelState.IsValid)
            {
                string userName = HttpContext.User.Identity.Name;
                var student = this.UserManager.Users.FirstOrDefault(s => s.UserName == userName);
                var result =await this.UserManager.ChangePasswordAsync(student,model.OriginalPassword,model.ModifiedPassword);
                if (result.Succeeded) //修改密码成功则登出，显示再次登录界面
                {
                    await this.SignInManager.SignOutAsync();
                    return this.View("ModifySuccess");
                }
                ModelState.AddModelError("","原密码输入错误");
            }
            return this.View(model);
        }
    }
}