namespace SysLibraryWeb.Models
{
    using System;

    public class PagingInfo
    {
        //总数
        public int TotalItems { get; set; }
        
        //每页数
        public int ItemsPerPage { get; set; }

        //目前的页数
        public int CurrentPage { get; set; }

        //总页数
        public int TotalPages
        {
            get => (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
        }
    }
}