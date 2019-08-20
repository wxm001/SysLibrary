namespace SysLibraryWeb.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum RetrieveType
    {
        UserName,
        Email
    }

    public class RetrieveViewModel
    {
        [Required]
        public RetrieveType RetrieveWay { get; set; }

        [Required]
        public string Account { get; set; }
    }
}