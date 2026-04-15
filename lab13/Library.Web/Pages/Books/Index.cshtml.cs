using Library.Application.DTOs;
using Library.Application.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Library.Web.Pages.Books;

public class IndexModel : PageModel
{
    private readonly IBookService _bookService;

    public IndexModel(IBookService bookService)
    {
        _bookService = bookService;
    }

    public List<BookResponseDto> Books { get; set; } = new();

    public void OnGet()
    {
        Books = _bookService.GetAll().ToList();
    }
}
