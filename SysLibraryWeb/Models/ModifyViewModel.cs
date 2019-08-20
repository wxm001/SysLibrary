namespace SysLibraryWeb.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ModifyViewModel
    {
        [UIHint("password")]
        [Display(Name = "原密码")]
        [Required]
        public string OriginalPassword { get; set; }

        [Required]
        [Display(Name = "新密码")]
        [UIHint("password")]
        public string ModifiedPassword { get; set; }

        [Required]
        [Display(Name = "确认密码")]
        [UIHint("password")]
        [Compare("ModifiedPassword",ErrorMessage = "两次密码不匹配")]
        public string ConfirmdPassword { get; set; }
    }
}