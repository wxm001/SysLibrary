using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SysLibraryWeb.Models
{
    //学生借书信息
    public class StudentInfo
    {
        [Key]
        public string UserName { get; set; }

        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 学位，用来限制借书数目
        /// </summary>
        [Required]
        public Degrees Degree { get; set; }

        /// <summary>
        /// 最大借书数目
        /// </summary>
        [Required]
        public int MaxBooksNumber { get; set; }

        /// <summary>
        /// 已借图书
        /// </summary>
        public ICollection<AppointmentOrLending> KeepingBooks { get; set; }

        public string AppointingBookBarCode { get; set; }

        [StringLength(14, MinimumLength = 11)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 罚款
        /// </summary>
        public decimal Fine { get; set; }

    }
}