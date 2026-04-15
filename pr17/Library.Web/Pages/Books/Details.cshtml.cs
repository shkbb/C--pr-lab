using Library.Application.DTOs;
using Library.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Library.Web.Pages.Books;

public class DetailsModel : PageModel
{
    private readonly IBookService _bookService;

    public DetailsModel(IBookService bookService)
    {
        _bookService = bookService;
    }

    public BookResponseDto Book { get; set; } = default!;

    public IActionResult OnGet(int id)
    {
        var book = _bookService.GetById(id);
        if (book == null)
        {
            return NotFound();
        }

        Book = book;
        return Page();
    }
}
