using AutoMapper;
using Library.Application.DTOs;
using Library.Application.Repositories;
using Library.Application.Services;
using Library.Domain.Entities;

namespace Library.Application.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _repository;
    private readonly IMapper _mapper;

    public BookService(IBookRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper     = mapper;
    }

    public IEnumerable<BookResponseDto> GetAll()
    {
        var books = _repository.GetAll();
        return _mapper.Map<IEnumerable<BookResponseDto>>(books);
    }

    public BookResponseDto? GetById(int id)
    {
        var book = _repository.GetById(id);
        return book is null ? null : _mapper.Map<BookResponseDto>(book);
    }

    public IEnumerable<BookResponseDto> Search(string keyword)
    {
        var books = string.IsNullOrWhiteSpace(keyword) 
                    ? _repository.GetAll() 
                    : _repository.SearchByTitle(keyword);
        return _mapper.Map<IEnumerable<BookResponseDto>>(books);
    }

    public BookResponseDto Create(BookCreateDto dto)
    {
        var entity = _mapper.Map<Book>(dto);
        _repository.Add(entity);
        _repository.SaveChanges();

        var created = _repository.GetById(entity.Id)!;
        return _mapper.Map<BookResponseDto>(created);
    }

    public bool Delete(int id)
    {
        var book = _repository.GetById(id);
        if (book is null) return false;
        _repository.Remove(book);
        _repository.SaveChanges();
        return true;
    }
}
