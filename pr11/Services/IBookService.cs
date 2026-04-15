using LibraryApi.Models;

namespace LibraryApi.Services;

// Service interface
public interface IBookService
{
    IEnumerable<Book> GetAll();
    Book? GetById(int id);
    Book Create(Book book);
}
