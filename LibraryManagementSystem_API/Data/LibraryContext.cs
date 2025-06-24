using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem_API.Models;

namespace LibraryManagementSystem_API.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options)//configure connection string, DB provider(SQLite here)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }//table for books
        public DbSet<User> Users { get; set; }//table for users
    }
}
