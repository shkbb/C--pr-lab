using Library.Domain.Entities;

namespace Library.Application.Repositories;

public interface IBookRepository
{
    IEnumerable<Book> GetAll();
    Book? GetById(int id);
    void Add(Book book);
    void Remove(Book book);
    void SaveChanges();
}
