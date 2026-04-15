using Library.Application.DTOs;
using Library.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Library.Web.Pages.Books;

public class CreateModel : PageModel
{
    private readonly IBookService _bookService;

    public CreateModel(IBookService bookService)
    {
        _bookService = bookService;
    }

    [BindProperty]
    public BookCreateDto Book { get; set; } = default!;

    public IActionResult OnGet()
    {
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try 
        {
            _bookService.Create(Book);
        } 
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, "Помилка при створенні. Можливо вказаний AuthorId не існує.");
            return Page();
        }

        return RedirectToPage("./Index");
    }
}
