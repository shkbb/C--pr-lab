using System.ComponentModel.DataAnnotations;

namespace LibraryApi.DTOs;

// DTO for creating/updating a book
public class BookCreateDto
{
    [Required(ErrorMessage = "Назва книги є обов'язковою")]
    [StringLength(200, MinimumLength = 1)]
    public string Title { get; set; } = string.Empty;

    [StringLength(20)]
    public string ISBN { get; set; } = string.Empty;

    [Range(1000, 2100, ErrorMessage = "Рік видання від 1000 до 2100")]
    public int PublishedYear { get; set; }

    [StringLength(50)]
    public string Genre { get; set; } = string.Empty;

    [Required]
    public int AuthorId { get; set; }
}

// DTO for responses (no navigation property — avoids cycles)
public class BookResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int PublishedYear { get; set; }
    public string Genre { get; set; } = string.Empty;
    public int AuthorId { get; set; }
    public string AuthorFullName { get; set; } = string.Empty;
}
