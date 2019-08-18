namespace SysLibraryWeb.Models
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Identity;
    //学生账户信息
    public class Student:IdentityUser
    {
        [ProtectedPersonalData]
        [RegularExpression("[UIA]\\d{9}")]
        [Display(Name = "学号")]
        public override string UserName { get; set; }

        [Display(Name = "手机号")]
        [StringLength(14,MinimumLength = 11)]
        public override string PhoneNumber { get; set; }

        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Display(Name = "学历")]
        public Degrees Degree { get; set; }

        [Display(Name = "最大借书数目")]
        public int MaxBooksNumber { get; set; }
    }
}