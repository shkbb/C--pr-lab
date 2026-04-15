using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("=== Library Management System — Практична 9 ===");
Console.WriteLine("Демонстрація CRUD-операцій через Entity Framework Core + SQL Server\n");

// Ensure database and schema are created
using (var ctx = new LibraryContext())
{
    ctx.Database.EnsureCreated();
}

// ─────────────────────────────────────────────────────────────────────────────
// CREATE
// ─────────────────────────────────────────────────────────────────────────────
Console.WriteLine("── CREATE ──────────────────────────────────────────────");

int authorId, book1Id, book2Id;

using (var ctx = new LibraryContext())
{
    // Clear previous demo data
    ctx.Books.RemoveRange(ctx.Books);
    ctx.Authors.RemoveRange(ctx.Authors);
    ctx.SaveChanges();

    var author1 = new Author { FirstName = "Тарас", LastName = "Шевченко", BirthYear = 1814 };
    var author2 = new Author { FirstName = "Іван", LastName = "Франко", BirthYear = 1856 };

    ctx.Authors.AddRange(author1, author2);
    ctx.SaveChanges();

    var book1 = new Book { Title = "Кобзар", ISBN = "978-966-01-0001-1", PublishedYear = 1840, AuthorId = author1.Id };
    var book2 = new Book { Title = "Лис Микита", ISBN = "978-966-01-0002-2", PublishedYear = 1890, AuthorId = author2.Id };
    var book3 = new Book { Title = "Захар Беркут", ISBN = "978-966-01-0003-3", PublishedYear = 1883, AuthorId = author2.Id };

    ctx.Books.AddRange(book1, book2, book3);
    ctx.SaveChanges();

    authorId = author1.Id;
    book1Id  = book1.Id;
    book2Id  = book2.Id;

    Console.WriteLine($"  Збережено автора:  {author1}");
    Console.WriteLine($"  Збережено автора:  {author2}");
    Console.WriteLine($"  Збережено книгу:   {book1}");
    Console.WriteLine($"  Збережено книгу:   {book2}");
    Console.WriteLine($"  Збережено книгу:   {book3}");
}

// ─────────────────────────────────────────────────────────────────────────────
// READ
// ─────────────────────────────────────────────────────────────────────────────
Console.WriteLine("\n── READ ─────────────────────────────────────────────────");

using (var ctx = new LibraryContext())
{
    var authors = ctx.Authors.Include(a => a.Books).ToList();
    foreach (var author in authors)
    {
        Console.WriteLine($"  Автор: {author}");
        foreach (var book in author.Books)
            Console.WriteLine($"    └─ Книга: {book}");
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// UPDATE
// ─────────────────────────────────────────────────────────────────────────────
Console.WriteLine("\n── UPDATE ───────────────────────────────────────────────");

using (var ctx = new LibraryContext())
{
    var book = ctx.Books.Find(book1Id);
    if (book is not null)
    {
        Console.WriteLine($"  До оновлення:    {book}");
        book.Title = "Кобзар (Повне видання)";
        book.PublishedYear = 1867;
        ctx.SaveChanges();
        Console.WriteLine($"  Після оновлення: {book}");
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// DELETE
// ─────────────────────────────────────────────────────────────────────────────
Console.WriteLine("\n── DELETE ───────────────────────────────────────────────");

using (var ctx = new LibraryContext())
{
    var book = ctx.Books.Find(book2Id);
    if (book is not null)
    {
        Console.WriteLine($"  Видалення книги: {book}");
        ctx.Books.Remove(book);
        ctx.SaveChanges();
        Console.WriteLine("  Книгу видалено успішно.");
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// FINAL STATE
// ─────────────────────────────────────────────────────────────────────────────
Console.WriteLine("\n── ПІДСУМКОВИЙ СТАН БД ──────────────────────────────────");

using (var ctx = new LibraryContext())
{
    var allBooks = ctx.Books.Include(b => b.Author).ToList();
    Console.WriteLine($"  Книг у базі: {allBooks.Count}");
    foreach (var b in allBooks)
        Console.WriteLine($"  {b} | Автор: {b.Author?.FirstName} {b.Author?.LastName}");
}

Console.WriteLine("\n=== Демонстрацію завершено ===");
