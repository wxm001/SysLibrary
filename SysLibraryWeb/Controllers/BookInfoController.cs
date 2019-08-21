using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SysLibraryWeb.Controllers
{
    using System.Text;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

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

        //首页
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

        //书籍的详细借阅信息,
        public IActionResult Detail(string isbn)
        {
            BookDetails bookDetails =
                this.LendingInfoDbContext.BookDetail.AsNoTracking().FirstOrDefault(b => b.ISBN == isbn);
            if (bookDetails==null)
            {
                TempData["message"] = $"找不到ISBN为{isbn}的书籍";
                return this.View("Index");
            }
            BookEditViewModel model=new BookEditViewModel()
            {
                BookDetails=bookDetails,
                Books = this.LendingInfoDbContext.Books.Include(b=>b.Appointments).AsNoTracking().Where(b=>b.ISBN==isbn)
            };
            return this.View(model);
        }

        //图书详细信息--管理员访问,
        [Authorize(Roles = "Admin")]
        public IActionResult BookDetails(string isbn, int page = 1)
        {
            IEnumerable<BookDetails> books = null;
            BookListViewModel model;
            if (HttpContext.Session!=null)
            {
                books = HttpContext.Session.Get<IEnumerable<BookDetails>>("bookDetails");
            }

            if (books==null)
            {
                books = this.LendingInfoDbContext.BookDetail.AsNoTracking();
                HttpContext.Session?.Set<IEnumerable<BookDetails>>("bookDetails",books);
            }

            if (isbn!=null) //若有isbn号，则显示相关书籍
            {
                model = new BookListViewModel()
                {
                    BookDetailses = new List<BookDetails>() { books.FirstOrDefault(b => b.ISBN == isbn) },
                    PagingInfo = new PagingInfo()
                };
                return this.View(model);
            }
            //若没有isbn，则顺序显示
            model=new BookListViewModel()
              {
                  PagingInfo = new PagingInfo()
                                   {
                                       ItemsPerPage = amout,
                                       TotalItems = books.Count(),
                                       CurrentPage = page
                                   },
                  BookDetailses = books.OrderBy(b=>b.FetchBookNumber).Skip((page-1)*amout).Take(amout)
              };
            return this.View(model);
        }

        //添加书籍信息视图
        [Authorize(Roles = "Admin")]
        public IActionResult AddBookDetails(BookDetails model)
        {
            if (model==null)
            {
                model = new BookDetails();
            }

            return this.View(model);
        }

        //添加书籍信息处理
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddBookDetails(BookDetails model, IFormFile image)
        {
            BookDetails bookDetails=new BookDetails();
            if (ModelState.IsValid)
            {
                if (image!=null)
                {
                    bookDetails.ImageMimeType = image.ContentType;
                    bookDetails.ImageData=new byte[image.Length];
                    await image.OpenReadStream().ReadAsync(bookDetails.ImageData, 0, (int)image.Length);
                }

                bookDetails.ISBN = model.ISBN;
                bookDetails.Name = model.Name;
                bookDetails.Author = model.Author;
                bookDetails.Description = model.Description;
                bookDetails.FetchBookNumber = model.FetchBookNumber;
                bookDetails.Press = model.Press;
                bookDetails.PublishDateTime = model.PublishDateTime;
                bookDetails.SoundCassettes = model.SoundCassettes;
                bookDetails.Version = model.Version;

                await this.LendingInfoDbContext.BookDetail.AddAsync(bookDetails);
                this.LendingInfoDbContext.SaveChanges();
                HttpContext.Session.Remove("bookDetails");//把session清除
                TempData["message"] = $"已添加书籍《{model.Name}》";
                return this.RedirectToAction("EditBookDetails");
            }

            return this.View(model);
        }

        //删除书籍信息处理
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveBooksAndBookDetails(IEnumerable<string> isbns)
        {
            StringBuilder sb=new StringBuilder();
            foreach (var isbn in isbns)
            {
                BookDetails bookDetails = this.LendingInfoDbContext.BookDetail.First(b => b.ISBN == isbn);
                IQueryable<Book> books = this.LendingInfoDbContext.Books.Where(b => b.ISBN == isbn);
                this.LendingInfoDbContext.BookDetail.Remove(bookDetails);
                this.LendingInfoDbContext.Books.RemoveRange(books);
                sb.Append("《" + bookDetails.Name + "》");
                await this.LendingInfoDbContext.SaveChangesAsync();
            }
            TempData["message"] = $"已移除书籍{sb.ToString()}";
            HttpContext.Session.Remove("bookDetails");//把session清除，不然还可以从session值拿到已删除的数据
            return this.RedirectToAction("BookDetails");
        }


        //编辑书籍信息
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditBookDetails(string isbn)
        {
            BookDetails bookDetails = await this.LendingInfoDbContext.BookDetail.FirstOrDefaultAsync(b => b.ISBN == isbn);
            if (bookDetails!=null)
            {
                return this.View(bookDetails);
            }
            else
            {
                return this.RedirectToAction("BookDetails");
            }
        }

        //编辑书籍信息处理
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditBookDetails(BookDetails model, IFormFile image)
        {
            BookDetails bookDetails = this.LendingInfoDbContext.BookDetail.FirstOrDefault(b => b.ISBN == model.ISBN);
            if (ModelState.IsValid)
            {
                if (bookDetails!=null)
                {
                    if (image!=null)
                    {
                        bookDetails.ImageMimeType = image.ContentType;
                        bookDetails.ImageData=new byte[image.Length];
                        await image.OpenReadStream().ReadAsync(bookDetails.ImageData, 0, (int)image.Length);
                    }

                    bookDetails.Name = model.Name;
                    bookDetails.Author = model.Author;
                    bookDetails.Description = model.Description;
                    bookDetails.FetchBookNumber = model.FetchBookNumber;
                    bookDetails.Press = model.Press;
                    bookDetails.PublishDateTime = model.PublishDateTime;
                    bookDetails.SoundCassettes = model.SoundCassettes;
                    bookDetails.Version = model.Version;

                    await this.LendingInfoDbContext.SaveChangesAsync();
                    HttpContext.Session.Remove("bookDetails");//把session清除
                    TempData["message"] = $"《{model.Name}》修改成功";
                    return this.RedirectToAction("EditBookDetails");
                }
            }

            return this.View(model);
        }

        //书籍检索
        public async Task<IActionResult> Search(string keyWord, string value)
        {
            BookDetails bookDetails=new BookDetails();
            switch (keyWord)
            {
                case "Name":
                    bookDetails =
                        await this.LendingInfoDbContext.BookDetail.FirstOrDefaultAsync(b => b.Name==value);
                    break;
                case "ISBN":
                    bookDetails =
                        await this.LendingInfoDbContext.BookDetail.FirstOrDefaultAsync(b => b.ISBN ==value);
                    break;
                case "FetchBookName":
                    bookDetails =
                        await this.LendingInfoDbContext.BookDetail.FirstOrDefaultAsync(b => b.FetchBookNumber ==value);
                    break;
            }

            if (bookDetails!=null)
            {
                return this.RedirectToAction("EditBookDetails", new { isbn = bookDetails.ISBN });
            }

            TempData["message"] = "找不到该书籍";
            return this.RedirectToAction("BookDetails");
        }
    }
}