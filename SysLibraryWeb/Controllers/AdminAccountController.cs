using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SysLibraryWeb.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using SysLibraryWeb.Models;
    using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

    [Authorize(Roles = "Admin")]
    public class AdminAccountController : Controller
    {
        private UserManager<Student> UserManager;

        public AdminAccountController(UserManager<Student> userManager)
        {
            this.UserManager = userManager;
        }

        //列表展示
        public IActionResult Index()
        {
            ICollection<Student> students = this.UserManager.Users.ToList();
            return this.View(students);
        }

        //添加学生
        [HttpPost]
        public async Task<JsonResult> AddStudent([FromBody]Student student)
        {

            if (this.UserManager.CreateAsync(student, "123456").Result.Succeeded)
            {
                return await addedStudent(student.UserName);
            }

            return Json("Failed");
        }
        //返回添加学生的json数据
        public async Task<JsonResult> addedStudent(string userName)
        {
            Student student = await this.UserManager.Users.FirstOrDefaultAsync(s => s.UserName == userName);
            return Json(
                new
                    {
                        userName = student.UserName,
                        name = student.Name,
                        degree =
                            student.Degree == Degrees.CollegeStudent
                                ? "本科生"
                                : (student.Degree == Degrees.Postgraduate ? "研究生" : "博士生"),
                        phoneNumber = student.PhoneNumber,
                        email = student.Email,
                        maxBooksNumber = student.MaxBooksNumber
                    });
        }

        //获取所有用户json型信息
        public JsonResult GetStudentData()
        {
            var students = this.UserManager.Users.Select(
                s => new
                 {
                     userName = s.UserName,
                     name = s.Name,
                     degree = s.Degree == Degrees.CollegeStudent
                                  ? "本科生"
                                  : (s.Degree == Degrees.Postgraduate ? "研究生" : "博士生"),
                     phoneNumber = s.PhoneNumber,
                     email = s.Email,
                     maxBooksNumber = s.MaxBooksNumber
                 });
            return Json(students);
        }

        //删除用户
        public async Task<JsonResult> RemoveStudent([FromBody] IEnumerable<string> userNames)
        {
            Student removeStudent;
            foreach (var userName in userNames)
            {
                removeStudent = await this.UserManager.FindByNameAsync(userName);
                if (removeStudent!=null)
                {
                    await this.UserManager.DeleteAsync(removeStudent);
                }
            }
            return this.GetStudentData();
        }
    }
}