using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SysLibraryWeb.Controllers
{
    using System.Net.Mail;
    using System.Text;

    using Microsoft.AspNetCore.Identity;

    using SysLibraryWeb.Infrastructure;
    using SysLibraryWeb.Models;

    public class PasswordRetrieverController : Controller
    {
        private UserManager<Student> UserManager;

        public EmailSender EmailSender;

        public PasswordRetrieverController(UserManager<Student> userManager,EmailSender emailSender)
        {
            this.UserManager = userManager;
            this.EmailSender = emailSender;
        }
        //重置密码视图1
        public IActionResult Retrieve()
        {
            RetrieveViewModel model = new RetrieveViewModel();
            return this.View(model);
        }

        //验证用户是否存在，生成重置密码的token并发送邮件
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RetrievePassword(RetrieveViewModel model)
        {
            bool sendResult = false;
            if (ModelState.IsValid)
            {
                Student student=new Student();
                switch (model.RetrieveWay)
                {
                    case RetrieveType.UserName: //通过用户名修改密码
                        student = await this.UserManager.FindByNameAsync(model.Account);
                        if (student!=null)
                        {
                            string code = await this.UserManager.GeneratePasswordResetTokenAsync(student);
                            sendResult = await SendEmail(student.Id, code, student.Email);
                        }
                        break;
                    case RetrieveType.Email:  //通过邮箱修改密码
                        student = await this.UserManager.FindByEmailAsync(model.Account);
                        if (student!=null)
                        {
                            string code = await this.UserManager.GeneratePasswordResetTokenAsync(student);
                            sendResult = await SendEmail(student.Id, code, student.Email);
                        }
                        break;
                }

                if (student==null)
                {
                    ViewBag.Error("用户不存在，请重新输入");
                    return this.View("Retrieve", model);
                }
            }

            ViewBag.Message = "已发送邮件至您的邮箱，请注意查收";
            ViewBag.Failed = "信息发送失败";
            return this.View(sendResult);
        }

        //发送邮件的方法
        async Task<bool> SendEmail(string userId, string code, string mailAddress)
        {
            Student student = await this.UserManager.FindByIdAsync(userId);
            if (student!=null)
            {
                string url = Url.Action(
                    "ResetPassword",
                    "PasswordRetriever",
                    new { userId = userId, code = code },
                    Url.ActionContext.HttpContext.Request.Scheme);
                StringBuilder sb=new StringBuilder();
                sb.AppendLine($"  请点击<a href=\"{url}\">此处</a>重置您的密码");
                MailMessage message=new MailMessage(from:"wxm0_0wxm@163.com",to:mailAddress,subject:"重置密码",body:sb.ToString());
                message.BodyEncoding=Encoding.UTF8;
                message.IsBodyHtml = true;
                try
                {
                    this.EmailSender.SmtpClient.Send(message);
                }
                catch (Exception e)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        //重置密码视图，有和之前的Url结合起来
        public IActionResult ResetPassword(string userId, string code)
        {
            ResetPasswordViewModel model = new ResetPasswordViewModel() { UserId = userId, Code = code };
            return this.View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = this.UserManager.FindByIdAsync(model.UserId);
                if (user!=null)
                {
                    var result = await this.UserManager.ResetPasswordAsync(user.Result, model.Code, model.Password);
                    if (result.Succeeded)
                    {
                        return this.RedirectToAction(nameof(ResetSuccess));
                    }
                }
            }
            return this.View(model);
        }

        public IActionResult ResetSuccess()
        {
            return this.View();
        }
    }
}