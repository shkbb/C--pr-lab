using System.ComponentModel.DataAnnotations;

namespace Library.Application.DTOs;

public class BookCreateDto
{
    [Required(ErrorMessage = "Назва книги є обов'язковою")]
    [StringLength(200, MinimumLength = 1)]
    public string Title { get; set; } = string.Empty;

    [StringLength(20)]
    public string ISBN { get; set; } = string.Empty;

    [Range(1000, 2100)]
    public int PublishedYear { get; set; }

    [StringLength(50)]
    public string Genre { get; set; } = string.Empty;

    [Required]
    public int AuthorId { get; set; }
}

public class BookUpdateDto : BookCreateDto
{
    [Required]
    public int Id { get; set; }
}

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
