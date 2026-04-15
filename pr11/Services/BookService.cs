using LibraryApi.Data;
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

    public IEnumerable<Book> GetAll() =>
        _context.Books.Include(b => b.Author).ToList();

    public Book? GetById(int id) =>
        _context.Books.Include(b => b.Author).FirstOrDefault(b => b.Id == id);

    public Book Create(Book book)
    {
        _context.Books.Add(book);
        _context.SaveChanges();
        return book;
    }
}
