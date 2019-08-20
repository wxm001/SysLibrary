using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SysLibraryWeb.Controllers
{
    using SysLibraryWeb.Data;
    using SysLibraryWeb.Infrastructure;
    using SysLibraryWeb.Models;

    public class BookInfoController : Controller
    {
        private LendingInfoDbContext LendingInfoDbContext;

        private static int amout = 4;

        public BookInfoController(LendingInfoDbContext lendingInfoDbContext)
        {
            this.LendingInfoDbContext = lendingInfoDbContext;
        }

        public IActionResult Index(string category,int page=1)
        {
            IEnumerable<BookDetails> books = null;
            //使用session获取书籍列表
            if (HttpContext.Session != null)
            {
                books = HttpContext.Session.Get<IEnumerable<BookDetails>>("bookDetails");
            }

            if (books==null)
            {
                books = this.LendingInfoDbContext.BookDetail;
                HttpContext.Session?.Set<IEnumerable<BookDetails>>("bookDetails", books);
            }

            BookListViewModel model = new BookListViewModel()
              {
                  PagingInfo = new PagingInfo()
                      {
                          ItemsPerPage = amout,
                          TotalItems = books.Count(),
                          CurrentPage = page
                      },
                  BookDetailses = books.OrderBy(b => b.FetchBookNumber).Skip((page - 1) * amout).Take(amout)
              };
                              
            return View(model);
        }

        //用来拿书封面
        public FileContentResult GetImage(string isbn)
        {
            BookDetails target = this.LendingInfoDbContext.BookDetail.FirstOrDefault(b => b.ISBN == isbn);
            if (target!=null)
            {
                return File(target.ImageData, target.ImageMimeType);
            }

            return null;
        }
    }
}