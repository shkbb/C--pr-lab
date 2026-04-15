using LibraryApp.Data;
using LibraryApp.Models;
using LibraryApp.Repositories;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("=== Library Management System — Лабораторна 10 ===");
Console.WriteLine("EF Core: 4 сутності, Repository, LINQ з Include/OrderBy/GroupBy\n");

var ctx = new LibraryContext();
ctx.Database.EnsureCreated();

ctx.Loans.RemoveRange(ctx.Loans);
ctx.Books.RemoveRange(ctx.Books);
ctx.Members.RemoveRange(ctx.Members);
ctx.Authors.RemoveRange(ctx.Authors);
ctx.SaveChanges();

var bookRepo = new BookRepository(ctx);

Console.WriteLine("── SEEDING ──────────────────────────────────────────────────");

var a1 = new Author { FirstName = "Тарас", LastName = "Шевченко", BirthYear = 1814 };
var a2 = new Author { FirstName = "Іван", LastName = "Франко", BirthYear = 1856 };
var a3 = new Author { FirstName = "Леся", LastName = "Українка", BirthYear = 1871 };
ctx.Authors.AddRange(a1, a2, a3);
ctx.SaveChanges();

var b1 = new Book { Title = "Кобзар", ISBN = "978-966-01-0001", PublishedYear = 1840,
                    Genre = "Поезія", IsAvailable = true, AuthorId = a1.Id };
var b2 = new Book { Title = "Лис Микита", ISBN = "978-966-01-0002", PublishedYear = 1890,
                    Genre = "Байка", IsAvailable = true, AuthorId = a2.Id };
var b3 = new Book { Title = "Захар Беркут", ISBN = "978-966-01-0003", PublishedYear = 1883,
                    Genre = "Роман", IsAvailable = false, AuthorId = a2.Id };
var b4 = new Book { Title = "Лісова пісня", ISBN = "978-966-01-0004", PublishedYear = 1911,
                    Genre = "Драма", IsAvailable = true, AuthorId = a3.Id };
var b5 = new Book { Title = "Гайдамаки", ISBN = "978-966-01-0005", PublishedYear = 1841,
                    Genre = "Поезія", IsAvailable = true, AuthorId = a1.Id };
ctx.Books.AddRange(b1, b2, b3, b4, b5);
ctx.SaveChanges();

var m1 = new Member { FullName = "Олена Петренко", Email = "olena@lib.ua", JoinDate = new DateTime(2023, 1, 15) };
var m2 = new Member { FullName = "Андрій Коваль", Email = "andrii@lib.ua", JoinDate = new DateTime(2024, 3, 20) };
ctx.Members.AddRange(m1, m2);
ctx.SaveChanges();

var loan1 = new Loan { BookId = b3.Id, MemberId = m1.Id, LoanDate = DateTime.Today.AddDays(-10) };
var loan2 = new Loan { BookId = b1.Id, MemberId = m2.Id, LoanDate = DateTime.Today.AddDays(-2),
                       ReturnDate = DateTime.Today };
ctx.Loans.AddRange(loan1, loan2);
ctx.SaveChanges();

Console.WriteLine($"  Авторів: {ctx.Authors.Count()}, Книг: {ctx.Books.Count()}, " +
                  $"Читачів: {ctx.Members.Count()}, Позик: {ctx.Loans.Count()}");

Console.WriteLine("\n── READ з Include (книги з авторами) ────────────────────────");
foreach (var b in bookRepo.GetWithAuthor())
    Console.WriteLine($"  {b} | Автор: {b.Author?.FullName}");

Console.WriteLine("\n── ФІЛЬТРАЦІЯ за жанром «Поезія» ────────────────────────────");
foreach (var b in bookRepo.GetByGenre("Поезія"))
    Console.WriteLine($"  {b}");

Console.WriteLine("\n── ПОШУК за ключовим словом «ліс» ──────────────────────────");
foreach (var b in bookRepo.SearchByTitle("ліс"))
    Console.WriteLine($"  {b}");

Console.WriteLine("\n── GroupBy: книги за жанром ─────────────────────────────────");
var byGenre = ctx.Books
    .GroupBy(b => b.Genre)
    .Select(g => new { Genre = g.Key, Count = g.Count() })
    .OrderByDescending(g => g.Count)
    .ToList();

foreach (var g in byGenre)
    Console.WriteLine($"  {g.Genre}: {g.Count} кн.");

Console.WriteLine("\n── Позики з Include (книга + автор + читач) ─────────────────");
var loans = ctx.Loans
    .Include(l => l.Book)
        .ThenInclude(b => b!.Author)
    .Include(l => l.Member)
    .ToList();

foreach (var l in loans)
    Console.WriteLine($"  {l}\n    Книга: \"{l.Book?.Title}\" (автор: {l.Book?.Author?.FullName})" +
                      $"\n    Читач: {l.Member?.FullName}");

Console.WriteLine("\n── UPDATE через репозиторій ─────────────────────────────────");
var bookToUpdate = bookRepo.GetById(b2.Id)!;
Console.WriteLine($"  До: {bookToUpdate}");
bookToUpdate.Genre = "Поема";
bookToUpdate.IsAvailable = false;
bookRepo.Update(bookToUpdate);
bookRepo.Save();
Console.WriteLine($"  Після: {bookToUpdate}");

Console.WriteLine("\n── DELETE через репозиторій ─────────────────────────────────");
Console.WriteLine($"  Видаляємо книгу #{b5.Id}: \"{b5.Title}\"");
bookRepo.Delete(b5.Id);
bookRepo.Save();
Console.WriteLine($"  Книг залишилось: {ctx.Books.Count()}");

Console.WriteLine("\n=== Лабораторну 10 завершено ===");
ctx.Dispose();
