using AutoMapper;
using Library.Application.DTOs;
using Library.Domain.Entities;

namespace Library.Application.Mapping;

public class BookMappingProfile : Profile
{
    public BookMappingProfile()
    {
        // Entity → ResponseDto
        CreateMap<Book, BookResponseDto>()
            .ForMember(dest => dest.AuthorFullName,
                       opt  => opt.MapFrom(src => src.Author != null
                                                  ? $"{src.Author.FirstName} {src.Author.LastName}"
                                                  : string.Empty));

        // CreateDto → Entity
        CreateMap<BookCreateDto, Book>()
            .ForMember(dest => dest.Id,     opt => opt.Ignore())
            .ForMember(dest => dest.Author, opt => opt.Ignore());

        // UpdateDto -> Entity
        CreateMap<BookUpdateDto, Book>()
            .ForMember(dest => dest.Author, opt => opt.Ignore());
    }
}
