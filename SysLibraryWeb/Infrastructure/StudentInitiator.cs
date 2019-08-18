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

            if (userManager.Users.Any())
            {
                return;
            }

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
                       PhoneNumber = "15610520652",
                       Degree = Degrees.DoctorateDegree,
                       MaxBooksNumber = 20
                   },
               };
            foreach (var student in initialStudents)
            {
                await userManager.CreateAsync(student, student.UserName.Substring(student.UserName.Length - 6, 6));
            }
        }
    }
}