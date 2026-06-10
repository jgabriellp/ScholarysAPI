using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs.Aluno;
using SchoolAPI.Services;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AlunoController : ControllerBase
{
    private readonly AlunoService _service;

    public AlunoController(AlunoService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Diretor,Coordenador")]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var (data, total) = await _service.GetAllAsync(page, pageSize);
        return Ok(new { data, total, page, pageSize });
    }

    [HttpGet("turma/{turmaId}")]
    [Authorize(Roles = "Admin,Diretor,Coordenador,Professor")]
    public async Task<IActionResult> GetByTurma(int turmaId)
    {
        var data = await _service.GetByTurmaAsync(turmaId);
        return Ok(data);
    }

    [HttpGet("usuario/{userId}")]
    public async Task<IActionResult> GetByUsuario(int userId)
    {
        var callerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var callerRole = User.FindFirstValue(ClaimTypes.Role);
        var isAdmin = callerRole is "Admin" or "Diretor" or "Coordenador";

        if (!isAdmin && callerId != userId.ToString())
            return Forbid();

        var aluno = await _service.GetByUsuarioAsync(userId);
        if (aluno == null) return NotFound();
        return Ok(aluno);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var aluno = await _service.GetByIdAsync(id);
        if (aluno == null) return NotFound();
        return Ok(aluno);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] AlunoRequestDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] AlunoRequestDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        if (updated == null) return NotFound();
        return Ok(updated);
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