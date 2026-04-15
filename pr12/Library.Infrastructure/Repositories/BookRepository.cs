using Library.Application.Repositories;
using Library.Domain.Entities;
using Library.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
    private readonly LibraryContext _context;

    public BookRepository(LibraryContext context)
    {
        _context = context;
    }

    public IEnumerable<Book> GetAll() =>
        _context.Books.Include(b => b.Author).ToList();

    public Book? GetById(int id) =>
        _context.Books.Include(b => b.Author).FirstOrDefault(b => b.Id == id);

    public void Add(Book book) =>
        _context.Books.Add(book);

    public void Remove(Book book) =>
        _context.Books.Remove(book);

    public void SaveChanges() =>
        _context.SaveChanges();
}
