using BookStoreExample.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreExample.Repository
{
    public class BookStoreDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Publisher> Publishers { get; set; }

        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookAuthors>()
                .HasKey(t => new { t.AuthorId, t.BookId });
            modelBuilder.Entity<BookGenre>()
                .HasKey(t => new { t.GenreId, t.BookId });
        }
    }
}