using System;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DemoExceptionsController : ControllerBase
{
    [HttpGet("bad-request")]
    public IActionResult ThrowBadRequest()
    {
        throw new ArgumentException("Некоректний параметр запиту: ID не може бути від'ємним.");
    }

    [HttpGet("business-rule")]
    public IActionResult ThrowBusinessRule()
    {
        throw new InvalidOperationException("Порушення бізнес-правила: неможливо створити об'єкт з некоректним станом.");
    }

    [HttpGet("data-access")]
    public IActionResult ThrowDataAccess()
    {
        throw new Exception("Виняток на рівні доступу до даних: втрачено з'єднання з БД.");
    }
}
