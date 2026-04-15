using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Repositories;

public class BookRepository : IBookRepository
{
    private readonly LibraryContext _context;

    public BookRepository(LibraryContext context)
    {
        _context = context;
    }

    public IEnumerable<Book> GetAll() => _context.Books.ToList();

    public Book? GetById(int id) => _context.Books
        .Include(b => b.Author)
        .FirstOrDefault(b => b.Id == id);

    public void Add(Book entity)
    {
        _context.Books.Add(entity);
    }

    public void Update(Book entity)
    {
        _context.Books.Update(entity);
    }

    public void Delete(int id)
    {
        var book = _context.Books.Find(id);
        if (book is not null) _context.Books.Remove(book);
    }

    public void Save() => _context.SaveChanges();

    // ── LINQ queries ─────────────────────────────────────────────────────────

    public IEnumerable<Book> GetByGenre(string genre) =>
        _context.Books
            .Where(b => b.Genre == genre)
            .Include(b => b.Author)
            .OrderBy(b => b.Title)
            .ToList();

    public IEnumerable<Book> GetAvailable() =>
        _context.Books
            .Where(b => b.IsAvailable)
            .Include(b => b.Author)
            .ToList();

    public IEnumerable<Book> GetWithAuthor() =>
        _context.Books
            .Include(b => b.Author)
            .OrderBy(b => b.Author!.LastName)
            .ThenBy(b => b.Title)
            .ToList();

    public IEnumerable<Book> SearchByTitle(string keyword) =>
        _context.Books
            .Where(b => b.Title.Contains(keyword))
            .Include(b => b.Author)
            .ToList();
}
