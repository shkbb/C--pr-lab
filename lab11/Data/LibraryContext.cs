using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Data;

public class LibraryContext : DbContext
{
    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .HasOne(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        
        modelBuilder.Entity<Author>().HasData(
            new Author { Id = 1, FirstName = "Тарас", LastName = "Шевченко", BirthYear = 1814 },
            new Author { Id = 2, FirstName = "Іван", LastName = "Франко", BirthYear = 1856 }
        );
        modelBuilder.Entity<Book>().HasData(
            new Book { Id = 1, Title = "Кобзар", ISBN = "978-966-01-0001", PublishedYear = 1840, Genre = "Поезія", AuthorId = 1 },
            new Book { Id = 2, Title = "Лис Микита", ISBN = "978-966-01-0002", PublishedYear = 1890, Genre = "Байка", AuthorId = 2 }
        );
    }
}
