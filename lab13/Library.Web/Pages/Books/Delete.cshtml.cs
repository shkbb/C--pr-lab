using Library.Application.DTOs;
using Library.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Library.Web.Pages.Books;

public class DeleteModel : PageModel
{
    private readonly IBookService _bookService;

    public DeleteModel(IBookService bookService)
    {
        _bookService = bookService;
    }

    [BindProperty]
    public BookResponseDto Book { get; set; } = default!;

    public IActionResult OnGet(int id)
    {
        var book = _bookService.GetById(id);
        if (book == null)
            return NotFound();

        Book = book;
        return Page();
    }

    public IActionResult OnPost()
    {
        var success = _bookService.Delete(Book.Id);
        if (!success)
            return NotFound();

        return RedirectToPage("./Index");
    }
}
