using LibraryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Repositories;

// Generic repository interface
public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll();
    T? GetById(int id);
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
    void Save();
}

// Book-specific repository with LINQ queries
public interface IBookRepository : IRepository<Book>
{
    IEnumerable<Book> GetByGenre(string genre);
    IEnumerable<Book> GetAvailable();
    IEnumerable<Book> GetWithAuthor();
    IEnumerable<Book> SearchByTitle(string keyword);
}
