using Library.Application.DTOs;
using Library.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    /// <summary>
    /// Отримати всі книги
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BookResponseDto>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        return Ok(_bookService.GetAll());
    }

    /// <summary>
    /// Отримати книгу за ідентифікатором
    /// </summary>
    /// <param name="id">Id книги</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BookResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var book = _bookService.GetById(id);
        if (book is null)
            return NotFound(new { message = $"Book with id {id} not found." });
            
        return Ok(book);
    }

    /// <summary>
    /// Знайти книги за назвою
    /// </summary>
    /// <param name="title">Назва книги або її частина</param>
    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<BookResponseDto>), StatusCodes.Status200OK)]
    public IActionResult Search([FromQuery] string title)
    {
        return Ok(_bookService.Search(title));
    }

    /// <summary>
    /// Створити нову книгу
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(BookResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] BookCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = _bookService.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Видалити книгу
    /// </summary>
    /// <param name="id">Id книги для видалення</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        if (!_bookService.Delete(id))
            return NotFound(new { message = $"Book with id {id} not found." });

        return NoContent();
    }
}
