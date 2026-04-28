using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs.Nota;
using SchoolAPI.Services;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotaController : ControllerBase
{
    private readonly NotaService _service;

    public NotaController(NotaService service)
    {
        _service = service;
    }

    [HttpGet("aluno/{alunoId}/ano/{anoLetivoId}")]
    public async Task<IActionResult> GetByAluno(int alunoId, int anoLetivoId)
    {
        var data = await _service.GetByAlunoAsync(alunoId, anoLetivoId);
        return Ok(data);
    }

    [HttpGet("turma/{turmaId}/disciplina/{disciplinaId}/ano/{anoLetivoId}")]
    [Authorize(Roles = "Admin,Diretor,Coordenador,Professor")]
    public async Task<IActionResult> GetByTurmaEDisciplina(int turmaId, int disciplinaId, int anoLetivoId)
    {
        var data = await _service.GetByTurmaEDisciplinaAsync(turmaId, disciplinaId, anoLetivoId);
        return Ok(data);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Professor")]
    public async Task<IActionResult> Lancar([FromBody] NotaRequestDto dto)
    {
        try
        {
            var result = await _service.LancarAsync(dto);
            return Ok(result);
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

    [HttpGet("media/aluno/{alunoId}/disciplina/{disciplinaId}/ano/{anoLetivoId}")]
    public async Task<IActionResult> CalcularMedia(int alunoId, int disciplinaId, int anoLetivoId)
    {
        var media = await _service.CalcularMediaAsync(alunoId, disciplinaId, anoLetivoId);
        return Ok(media);
    }
}