using Library.Domain.Entities;

namespace Library.Application.Repositories;

public interface IBookRepository
{
    IEnumerable<Book> GetAll();
    Book? GetById(int id);
    IEnumerable<Book> SearchByTitle(string keyword);
    void Add(Book book);
    void Update(Book book);
    void Remove(Book book);
    void SaveChanges();
}
