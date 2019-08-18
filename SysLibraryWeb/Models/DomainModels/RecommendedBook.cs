using System;
using System.ComponentModel.DataAnnotations;

namespace SysLibraryWeb.Models
{
    public class RecommendedBook
    {
        [Key]
        public string ISBN { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string Press { get; set; }

        /// <summary>
        /// 出版时间
        /// </summary>
        [Required]
        public DateTime PublishDateTime { get; set; }

        /// <summary>
        /// 书籍版本
        /// </summary>
        [Required]
        public int Version { get; set; }

        /// <summary>
        /// 载体形态，包括页数、媒介等信息
        /// </summary>
        public string SoundCassettes { get; set; }
    }
}