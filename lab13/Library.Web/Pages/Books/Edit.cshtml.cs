using Library.Application.DTOs;
using Library.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Library.Web.Pages.Books;

public class EditModel : PageModel
{
    private readonly IBookService _bookService;

    public EditModel(IBookService bookService)
    {
        _bookService = bookService;
    }

    [BindProperty]
    public BookUpdateDto Book { get; set; } = default!;

    public IActionResult OnGet(int id)
    {
        var book = _bookService.GetById(id);
        if (book == null)
            return NotFound();

        Book = new BookUpdateDto
        {
            Id = book.Id,
            Title = book.Title,
            ISBN = book.ISBN,
            PublishedYear = book.PublishedYear,
            Genre = book.Genre,
            AuthorId = book.AuthorId
        };
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        try 
        {
            if (!_bookService.Update(Book.Id, Book))
                return NotFound();
        } 
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, "Помилка при оновленні. Можливо вказаний AuthorId не існує.");
            return Page();
        }

        return RedirectToPage("./Index");
    }
}
