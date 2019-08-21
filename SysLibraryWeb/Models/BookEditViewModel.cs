namespace SysLibraryWeb.Models
{
    using System.Collections.Generic;

    public class BookEditViewModel
    {
        public BookDetails BookDetails { get; set; }

        public IEnumerable<Book> Books { get; set; }
    }
}