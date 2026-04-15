using LibraryApi.Data;
using LibraryApi.DTOs;
using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Services;

public class BookService : IBookService
{
    private readonly LibraryContext _context;

    public BookService(LibraryContext context)
    {
        _context = context;
    }

    public IEnumerable<BookResponseDto> GetAll() =>
        _context.Books
            .Include(b => b.Author)
            .Select(b => ToDto(b))
            .ToList();

    public BookResponseDto? GetById(int id)
    {
        var book = _context.Books.Include(b => b.Author).FirstOrDefault(b => b.Id == id);
        return book is null ? null : ToDto(book);
    }

    public BookResponseDto Create(BookCreateDto dto)
    {
        var book = new Book
        {
            Title        = dto.Title,
            ISBN         = dto.ISBN,
            PublishedYear = dto.PublishedYear,
            Genre        = dto.Genre,
            AuthorId     = dto.AuthorId
        };
        _context.Books.Add(book);
        _context.SaveChanges();

        // Reload with author navigation
        _context.Entry(book).Reference(b => b.Author).Load();
        return ToDto(book);
    }

    public bool Delete(int id)
    {
        var book = _context.Books.Find(id);
        if (book is null) return false;
        _context.Books.Remove(book);
        _context.SaveChanges();
        return true;
    }

    private static BookResponseDto ToDto(Book b) => new()
    {
        Id             = b.Id,
        Title          = b.Title,
        ISBN           = b.ISBN,
        PublishedYear  = b.PublishedYear,
        Genre          = b.Genre,
        AuthorId       = b.AuthorId,
        AuthorFullName = b.Author is null ? "" : $"{b.Author.FirstName} {b.Author.LastName}"
    };
}
