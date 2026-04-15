using AutoMapper;
using Library.Application.DTOs;
using Library.Application.Repositories;
using Library.Application.Services;
using Library.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Library.Application.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<BookService> _logger;
    private readonly IMemoryCache _cache;
    private const string CacheKey = "GetAllBooks";

    public BookService(IBookRepository repository, IMapper mapper, ILogger<BookService> logger, IMemoryCache cache)
    {
        _repository = repository;
        _mapper     = mapper;
        _logger     = logger;
        _cache      = cache;
    }

    public IEnumerable<BookResponseDto> GetAll()
    {
        _logger.LogInformation("Отримання списку всіх книг.");
        
        if (!_cache.TryGetValue(CacheKey, out IEnumerable<BookResponseDto>? cachedBooks))
        {
            _logger.LogInformation("Cache miss. Завантаження книг з БД.");
            var books = _repository.GetAll();
            cachedBooks = _mapper.Map<IEnumerable<BookResponseDto>>(books);
            
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                
            _cache.Set(CacheKey, cachedBooks, cacheOptions);
        }
        else
        {
            _logger.LogInformation("Cache hit. Повернення книг з кешу.");
        }

        return cachedBooks!;
    }

    public BookResponseDto? GetById(int id)
    {
        _logger.LogInformation("Отримання книги за ID={Id}", id);
        var book = _repository.GetById(id);
        if (book is null)
        {
            _logger.LogWarning("Книгу з ID={Id} не знайдено.", id);
            return null;
        }
        return _mapper.Map<BookResponseDto>(book);
    }

    public IEnumerable<BookResponseDto> Search(string keyword)
    {
        _logger.LogInformation("Пошук книг за ключовим словом: {Keyword}", keyword);
        var books = string.IsNullOrWhiteSpace(keyword) 
                    ? _repository.GetAll() 
                    : _repository.SearchByTitle(keyword);
        return _mapper.Map<IEnumerable<BookResponseDto>>(books);
    }

    public BookResponseDto Create(BookCreateDto dto)
    {
        _logger.LogInformation("Створення нової книги з назвою {Title}", dto.Title);
        var entity = _mapper.Map<Book>(dto);
        _repository.Add(entity);
        _repository.SaveChanges();
        
        _logger.LogInformation("Книгу {Title} створено з ID={Id}", dto.Title, entity.Id);
        
        // Invalidate cache
        _cache.Remove(CacheKey);

        var created = _repository.GetById(entity.Id)!;
        return _mapper.Map<BookResponseDto>(created);
    }

    public bool Delete(int id)
    {
        _logger.LogInformation("Видалення книги з ID={Id}", id);
        var book = _repository.GetById(id);
        if (book is null) 
        {
            _logger.LogWarning("Неможливо видалити. Книгу з ID={Id} не знайдено.", id);
            return false;
        }
        
        _repository.Remove(book);
        _repository.SaveChanges();
        
        _logger.LogInformation("Книгу з ID={Id} успішно видалено.", id);
        
        // Invalidate cache
        _cache.Remove(CacheKey);
        
        return true;
    }
}
