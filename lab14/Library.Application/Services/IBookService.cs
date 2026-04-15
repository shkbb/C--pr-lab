using Library.Application.DTOs;

namespace Library.Application.Services;

public interface IBookService
{
    IEnumerable<BookResponseDto> GetAll();
    BookResponseDto? GetById(int id);
    IEnumerable<BookResponseDto> Search(string keyword);
    BookResponseDto Create(BookCreateDto dto);
    bool Update(int id, BookUpdateDto dto);
    bool Delete(int id);
}
