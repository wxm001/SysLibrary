namespace SysLibraryWeb.Models
{
    using System.ComponentModel.DataAnnotations;

    //书籍状态枚举
    public enum BookState
    {
        /// <summary>
        /// 可借阅
        /// </summary>
        [Display(Name = "正常")]
        Normal,
        /// <summary>
        /// 管内阅览
        /// </summary>
        [Display(Name = "管内阅览")]
        Readonly,
        /// <summary>
        /// 已借出
        /// </summary>
        [Display(Name = "已借出")]
        Borrowed,
        /// <summary>
        /// 被续借
        /// </summary>
        [Display(Name = "被续借")]
        ReBorrowed,
        /// <summary>
        /// 被预约
        /// </summary>
        [Display(Name = "被预约")]
        Appointed,
        /// <summary>
        /// 过期
        /// </summary>
        [Display(Name = "过期")]
        Expired
    }
}