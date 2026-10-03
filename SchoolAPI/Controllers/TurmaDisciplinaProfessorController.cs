using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs.TurmaDisciplinaProfessor;
using SchoolAPI.Services;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TurmaDisciplinaProfessorController : ControllerBase
{
    private readonly TurmaDisciplinaProfessorService _service;

    public TurmaDisciplinaProfessorController(TurmaDisciplinaProfessorService service)
    {
        _service = service;
    }

    [HttpGet("turma/{turmaId}")]
    [Authorize(Roles = "Admin,Diretor,Coordenador,Professor")]
    public async Task<IActionResult> GetByTurma(int turmaId)
    {
        var data = await _service.GetByTurmaAsync(turmaId);
        return Ok(data);
    }

    [HttpGet("professor/{professorId}")]
    [Authorize(Roles = "Admin,Diretor,Coordenador,Professor")]
    public async Task<IActionResult> GetByProfessor(int professorId)
    {
        var data = await _service.GetByProfessorAsync(professorId);
        return Ok(data);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Diretor,Coordenador")]
    public async Task<IActionResult> GetById(int id)
    {
        var data = await _service.GetByIdAsync(id);
        if (data == null) return NotFound();
        return Ok(data);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] TurmaDisciplinaProfessorRequestDto dto)
    {
        try
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] TurmaDisciplinaProfessorRequestDto dto)
    {
        try
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}