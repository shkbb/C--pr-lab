using LibraryApi.DTOs;

namespace LibraryApi.Services;

public interface IBookService
{
    IEnumerable<BookResponseDto> GetAll();
    BookResponseDto? GetById(int id);
    BookResponseDto Create(BookCreateDto dto);
    bool Delete(int id);
}
