using BackEnd.Modules.Authors.Dtos;
using BackEnd.Modules.Authors.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Modules.Authors.Controllers;

[ApiController]
[Route("api/authors")]
[Authorize]
public class AuthorsController(IAuthorService authors) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<AuthorDto>>> GetAll()
    {
        return Ok(await authors.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AuthorDto>> GetById(int id)
    {
        var author = await authors.GetByIdAsync(id);
        if (author is null) return NotFound();
        return Ok(author);
    }

    [HttpPost]
    public async Task<ActionResult<AuthorDto>> Create(AuthorRequestDto dto)
    {
        var created = await authors.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, AuthorRequestDto dto)
    {
        var updated = await authors.UpdateAsync(id, dto);
        if (!updated) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await authors.DeleteAsync(id);

        if (result == DeleteResult.NotFound) return NotFound();
        if (result == DeleteResult.HasBooks)
            return Conflict(new { message = "No se puede eliminar: el autor tiene libros asociados." });

        return NoContent();
    }
}
