namespace SysLibraryWeb.Models
{
    using System.Collections.Generic;

    public class BookListViewModel
    {
        public IEnumerable<BookDetails> BookDetailses { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}