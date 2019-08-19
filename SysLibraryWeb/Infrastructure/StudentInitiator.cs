namespace SysLibraryWeb.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore.Internal;
    using Microsoft.Extensions.DependencyInjection;

    using SysLibraryWeb.Models;

    public class StudentInitiator
    {
        //初始化用户信息数据库
        public static async Task Initial(IServiceProvider serviceProvider)
        {
            UserManager<Student> userManager = serviceProvider.GetRequiredService<UserManager<Student>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            if (userManager.Users.Any())
            {
                return;
            }

            //权限
            if (await roleManager.FindByNameAsync("Admin")==null)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (await roleManager.FindByNameAsync("Student")==null)
            {
                await roleManager.CreateAsync(new IdentityRole("Student"));
            }

            //初始化普通用户
            IEnumerable<Student> initialStudents = new[]
               {
                   new Student()
                   {
                       UserName = "U201900001",
                       Name = "wei",
                       Email = "645954833@qq.com",
                       PhoneNumber = "15610520652",
                       Degree = Degrees.Postgraduate,
                       MaxBooksNumber = 15
                   },
                   new Student()
                   {
                       UserName = "U201900002",
                       Name = "zhang",
                       Email = "1152072020@qq.com",
                       PhoneNumber = "15610056015",
                       Degree = Degrees.DoctorateDegree,
                       MaxBooksNumber = 20
                   },
               };

            //初始化管理员
            IEnumerable<Student> initialAdmins = new[]
             {
                 new Student()
                     {
                         UserName = "A000000000",
                         Name = "Admin0000",
                         Email = "Admin1@cnblog.com",
                         PhoneNumber = "12345678912",
                         Degree = Degrees.CollegeStudent,
                         MaxBooksNumber = 10
                     },
                 new Student()
                     {
                         UserName = "A000000001",
                         Name = "Admin0001",
                         Email = "Admin2@cnblog.com",
                         PhoneNumber = "12345678910",
                         Degree = Degrees.CollegeStudent,
                         MaxBooksNumber = 10
                     }
             };

            foreach (var student in initialStudents)
            {
                await userManager.CreateAsync(student, student.UserName.Substring(student.UserName.Length - 6, 6));
            }

            foreach (var admin in initialAdmins)
            {
                await userManager.CreateAsync(admin, "Ws..951014");
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}