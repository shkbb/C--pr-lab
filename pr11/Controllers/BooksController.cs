using LibraryApi.Models;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    /// <summary>Отримати список всіх книг</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Book>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        var books = _bookService.GetAll();
        return Ok(books);
    }

    /// <summary>Отримати книгу за ідентифікатором</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Book), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var book = _bookService.GetById(id);
        if (book is null)
            return NotFound(new { message = $"Книгу з id={id} не знайдено." });
        return Ok(book);
    }

    /// <summary>Створити нову книгу</summary>
    [HttpPost]
    [ProducesResponseType(typeof(Book), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] Book book)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = _bookService.Create(book);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
}
