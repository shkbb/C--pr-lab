using LibraryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Data;

public class LibraryContext : DbContext
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Loan> Loans { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost,1433;Database=LibraryDB_lab10;User Id=sa;" +
            "Password=Your_Strong_Password_123!;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(e =>
        {
            e.HasKey(a => a.Id);
            e.Property(a => a.FirstName).IsRequired().HasMaxLength(50);
            e.Property(a => a.LastName).IsRequired().HasMaxLength(50);
            e.Ignore(a => a.FullName); // computed property, not stored
        });

        modelBuilder.Entity<Book>(e =>
        {
            e.HasKey(b => b.Id);
            e.Property(b => b.Title).IsRequired().HasMaxLength(200);
            e.Property(b => b.ISBN).HasMaxLength(20);
            e.Property(b => b.Genre).HasMaxLength(50);
            e.Ignore(b => b.IsAvailable); // stored as column via default
            e.Property<bool>("IsAvailable").HasDefaultValue(true);

            e.HasOne(b => b.Author)
             .WithMany(a => a.Books)
             .HasForeignKey(b => b.AuthorId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Member>(e =>
        {
            e.HasKey(m => m.Id);
            e.Property(m => m.FullName).IsRequired().HasMaxLength(100);
            e.Property(m => m.Email).HasMaxLength(100);
        });

        modelBuilder.Entity<Loan>(e =>
        {
            e.HasKey(l => l.Id);
            e.Ignore(l => l.IsReturned);

            e.HasOne(l => l.Book)
             .WithMany(b => b.Loans)
             .HasForeignKey(l => l.BookId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(l => l.Member)
             .WithMany(m => m.Loans)
             .HasForeignKey(l => l.MemberId)
             .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
