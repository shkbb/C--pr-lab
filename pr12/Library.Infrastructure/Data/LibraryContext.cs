using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Data;

public class LibraryContext : DbContext
{
    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(e =>
        {
            e.HasKey(a => a.Id);
            e.Property(a => a.FirstName).IsRequired().HasMaxLength(50);
            e.Property(a => a.LastName).IsRequired().HasMaxLength(50);
            e.Ignore(a => a.FullName);
        });

        modelBuilder.Entity<Book>(e =>
        {
            e.HasKey(b => b.Id);
            e.Property(b => b.Title).IsRequired().HasMaxLength(200);
            e.Property(b => b.ISBN).HasMaxLength(20);
            e.Property(b => b.Genre).HasMaxLength(50);

            e.HasOne(b => b.Author)
             .WithMany(a => a.Books)
             .HasForeignKey(b => b.AuthorId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // Seed data
        modelBuilder.Entity<Author>().HasData(
            new Author { Id = 1, FirstName = "Тарас", LastName = "Шевченко", BirthYear = 1814 },
            new Author { Id = 2, FirstName = "Іван",  LastName = "Франко",   BirthYear = 1856 }
        );
        modelBuilder.Entity<Book>().HasData(
            new Book { Id = 1, Title = "Кобзар",    ISBN = "978-966-01", PublishedYear = 1840, Genre = "Поезія", AuthorId = 1 },
            new Book { Id = 2, Title = "Лис Микита", ISBN = "978-966-02", PublishedYear = 1890, Genre = "Байка",  AuthorId = 2 }
        );
    }
}
