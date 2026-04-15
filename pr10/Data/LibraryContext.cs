using LibraryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Data;

public class LibraryContext : DbContext
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Member> Members { get; set; }   

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost,1433;Database=LibraryDB_pr10;User Id=sa;" +
            "Password=Your_Strong_Password_123!;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(a => a.LastName).IsRequired().HasMaxLength(50);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Title).IsRequired().HasMaxLength(200);
            entity.Property(b => b.ISBN).HasMaxLength(20);
            entity.Property(b => b.Genre).HasMaxLength(50).HasDefaultValue("Невизначений");

            entity.HasOne(b => b.Author)
                  .WithMany(a => a.Books)
                  .HasForeignKey(b => b.AuthorId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.FullName).IsRequired().HasMaxLength(100);
            entity.Property(m => m.Email).HasMaxLength(100);
        });
    }
}
