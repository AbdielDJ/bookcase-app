using BackEnd.Modules.Books.Dtos;
using BackEnd.Modules.Books.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Modules.Books.Controllers;

[ApiController]
[Route("api/books")]
[Authorize]
public class BooksController(IBookService books) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<BookDto>>> GetAll()
    {
        return Ok(await books.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookDto>> GetById(int id)
    {
        var book = await books.GetByIdAsync(id);
        if (book is null) return NotFound();
        return Ok(book);
    }

    [HttpPost]
    public async Task<ActionResult<BookDto>> Create(BookRequestDto dto)
    {
        var created = await books.CreateAsync(dto);
        if (created is null)
            return BadRequest(new { message = "El autor seleccionado no existe." });

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, BookRequestDto dto)
    {
        var result = await books.UpdateAsync(id, dto);

        if (result == BookUpdateResult.NotFound) return NotFound();
        if (result == BookUpdateResult.AuthorNotFound)
            return BadRequest(new { message = "El autor seleccionado no existe." });

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await books.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
