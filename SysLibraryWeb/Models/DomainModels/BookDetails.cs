using System;
using System.ComponentModel.DataAnnotations;

namespace SysLibraryWeb.Models
{
    //书籍自身详细信息
    public class BookDetails
    {
        [Key]
        public string ISBN { get; set; }
        [Display(Name = "书名")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "作者")]
        [Required]
        public string Author { get; set; }
        [Display(Name = "出版社")]
        [Required]
        public string Press { get; set; }
        [Display(Name = "取书号")]
        [Required]
        public string FetchBookNumber { get; set; }
        /// <summary>
        /// 出版时间
        /// </summary>
        [Display(Name = "出版时间")]
        [Required]
        public DateTime PublishDateTime { get; set; }

        /// <summary>
        /// 书籍版本
        /// </summary>
        [Display(Name = "版本")]
        [Required]
        public int Version { get; set; }

        /// <summary>
        /// 载体形态，包括页数、媒介等信息
        /// </summary>
        [Display(Name = "载体形态")]
        public string SoundCassettes { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        [Display(Name = "描述")]
        public string Description { get; set; }


        /// <summary>
        /// 缩略图
        /// </summary>
        [Display(Name = "缩略图")]
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }

    }
}