namespace SysLibraryWeb.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using SysLibraryWeb.Models;

    public class StudentIdentityDbContext:IdentityDbContext<Student>
    {
        public StudentIdentityDbContext(DbContextOptions<StudentIdentityDbContext> options)
            : base(options)
        {
        }
    }
}