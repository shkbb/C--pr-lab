using Library.Application.DTOs;

namespace Library.Application.Services;

public interface IBookService
{
    IEnumerable<BookResponseDto> GetAll();
    BookResponseDto? GetById(int id);
    BookResponseDto Create(BookCreateDto dto);
    bool Delete(int id);
}
