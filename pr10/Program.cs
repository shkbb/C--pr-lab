using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("=== Library Management System — Практична 10 ===");
Console.WriteLine("Демонстрація: міграції, нові сутності, change tracking, SaveChanges()\n");

// ─── Ensure schema is up-to-date (simulates applying migration) ───────────────
// In a real workflow: dotnet ef migrations add AddGenreAndMember
//                    dotnet ef database update
// Here we use EnsureCreated() for demo purposes.
using (var ctx = new LibraryContext())
{
    ctx.Database.EnsureCreated();
    // Clear previous data
    ctx.Books.RemoveRange(ctx.Books);
    ctx.Authors.RemoveRange(ctx.Authors);
    ctx.Members.RemoveRange(ctx.Members);
    ctx.SaveChanges();
}

// ─── CREATE (Short-lived context — pattern for short operations) ──────────────
Console.WriteLine("── Шаблон короткотривалого DbContext (CREATE) ──────────────");

int authorId;
using (var ctx = new LibraryContext())
{
    var author = new Author { FirstName = "Леся", LastName = "Українка", BirthYear = 1871 };
    ctx.Authors.Add(author);
    ctx.SaveChanges();
    authorId = author.Id;
    Console.WriteLine($"  Додано автора: {author}");
}

int book1Id, book2Id;
using (var ctx = new LibraryContext())
{
    var b1 = new Book { Title = "Лісова пісня", ISBN = "978-966-01-0010-3",
                         PublishedYear = 1911, Genre = "Драма", AuthorId = authorId };
    var b2 = new Book { Title = "Боярыня", ISBN = "978-966-01-0011-4",
                         PublishedYear = 1914, Genre = "Поема", AuthorId = authorId };
    ctx.Books.AddRange(b1, b2);
    ctx.SaveChanges();
    book1Id = b1.Id;
    book2Id = b2.Id;
    Console.WriteLine($"  Додано книгу: {b1}");
    Console.WriteLine($"  Додано книгу: {b2}");
}

// ─── NEW ENTITY: Member ───────────────────────────────────────────────────────
Console.WriteLine("\n── Нова сутність Member (з'явилась у міграції pr10) ─────────");
int memberId;
using (var ctx = new LibraryContext())
{
    var m = new Member { FullName = "Олена Петренко", Email = "olena@lib.ua", JoinDate = DateTime.Today };
    ctx.Members.Add(m);
    ctx.SaveChanges();
    memberId = m.Id;
    Console.WriteLine($"  Зареєстровано читача: {m}");
}

// ─── CHANGE TRACKING DEMO ────────────────────────────────────────────────────
Console.WriteLine("\n── Change Tracking та SaveChanges() ─────────────────────────");

using (var ctx = new LibraryContext())
{
    var book = ctx.Books.Find(book1Id)!;

    // Check initial state
    var entry = ctx.Entry(book);
    Console.WriteLine($"  Стан до зміни:  {entry.State}");

    book.Genre = "Драма-феєрія";  // Modify tracked entity

    Console.WriteLine($"  Стан після зміни: {entry.State}");
    Console.WriteLine($"  Стара назва жанру: {entry.OriginalValues[nameof(Book.Genre)]}");
    Console.WriteLine($"  Нова назва жанру:  {book.Genre}");

    ctx.SaveChanges();
    Console.WriteLine($"  Після SaveChanges(): {entry.State}");
}

// ─── UPDATE via short-lived context ──────────────────────────────────────────
Console.WriteLine("\n── UPDATE (short-lived context) ─────────────────────────────");

using (var ctx = new LibraryContext())
{
    var member = ctx.Members.Find(memberId)!;
    Console.WriteLine($"  До: {member}");
    member.Email = "olena.p@library.ua";
    ctx.SaveChanges();
    Console.WriteLine($"  Після: {member}");
}

// ─── DELETE ───────────────────────────────────────────────────────────────────
Console.WriteLine("\n── DELETE ───────────────────────────────────────────────────");

using (var ctx = new LibraryContext())
{
    var book = ctx.Books.Find(book2Id)!;
    Console.WriteLine($"  Видаляємо: {book}");
    ctx.Books.Remove(book);
    ctx.SaveChanges();
    Console.WriteLine("  Видалено.");
}

// ─── READ FINAL STATE ─────────────────────────────────────────────────────────
Console.WriteLine("\n── ПІДСУМКОВИЙ СТАН ─────────────────────────────────────────");

using (var ctx = new LibraryContext())
{
    Console.WriteLine("  Книги:");
    foreach (var b in ctx.Books.Include(b => b.Author).ToList())
        Console.WriteLine($"    {b} | Автор: {b.Author?.FirstName} {b.Author?.LastName}");

    Console.WriteLine("  Читачі:");
    foreach (var m in ctx.Members.ToList())
        Console.WriteLine($"    {m}");
}

Console.WriteLine("\n=== Демонстрацію завершено ===");
Console.WriteLine("Примітка: у реальному проєкті міграції застосовуються через:");
Console.WriteLine("  dotnet ef migrations add AddGenreAndMember");
Console.WriteLine("  dotnet ef database update");
