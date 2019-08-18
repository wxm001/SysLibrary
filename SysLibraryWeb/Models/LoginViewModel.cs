namespace SysLibraryWeb.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum LoginType
    {
        UserName,
        Email,
        Phone
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "请输入您的学号/邮箱/手机号码")]
        [Display(Name = "学号/邮箱/手机号码")]
        public string Account { get; set; }

        [Required(ErrorMessage = "请输入您的密码")]
        [UIHint("password")]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Required]
        public LoginType LoginType { get; set; }
    }
}