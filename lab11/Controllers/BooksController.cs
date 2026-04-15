using LibraryApi.DTOs;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    /// <summary>Отримати список всіх книг</summary>
    /// <returns>200 OK зі списком BookResponseDto</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BookResponseDto>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        return Ok(_bookService.GetAll());
    }

    /// <summary>Отримати книгу за ідентифікатором</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BookResponseDto), StatusCodes.Status200OK)]
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
    [ProducesResponseType(typeof(BookResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] BookCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = _bookService.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Видалити книгу за ідентифікатором</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var deleted = _bookService.Delete(id);
        if (!deleted)
            return NotFound(new { message = $"Книгу з id={id} не знайдено." });
        return NoContent();
    }
}
