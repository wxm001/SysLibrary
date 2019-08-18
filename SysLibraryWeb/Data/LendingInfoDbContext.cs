namespace SysLibraryWeb.Data
{
    using Microsoft.EntityFrameworkCore;

    using SysLibraryWeb.Models;

    public class LendingInfoDbContext:DbContext
    {
        public LendingInfoDbContext(DbContextOptions<LendingInfoDbContext> options):base(options)
        {
            
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<BookDetails> BookDetail { get; set; }

        public DbSet<Bookshelf> Bookshelves { get; set; }

        public DbSet<RecommendedBook> RecommendedBooks { get; set; }

        public DbSet<StudentInfo> Student { get; set; }

        public DbSet<AppointmentOrLending> AppointmentOrLendings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AppointmentOrLending>().HasKey(c => new { c.BookId, c.StudentId });
        }
    }
}