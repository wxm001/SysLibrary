namespace SysLibraryWeb.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum Degrees
    {
        // 学位枚举
        [Display(Name = "本科生")]
        CollegeStudent,
        [Display(Name = "研究生")]
        Postgraduate,
        [Display(Name = "博士生")]
        DoctorateDegree
    }
}