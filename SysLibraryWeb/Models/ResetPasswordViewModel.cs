namespace SysLibraryWeb.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ResetPasswordViewModel
    {
        //未使用token，创建专门用于重置密码的视图模型，其中Code用来接收生成的token，UserId用来传递待重置用户的id
        public string Code { get; set; }

        public string UserId { get; set; }

        [Required]
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "确认密码")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "两次密码不匹配")]
        public string ConfirmPassword { get; set; }
    }
}