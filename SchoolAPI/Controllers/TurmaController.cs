using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs.Turma;
using SchoolAPI.Services;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Diretor,Coordenador")]
public class TurmaController : ControllerBase
{
    private readonly TurmaService _service;

    public TurmaController(TurmaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var (data, total) = await _service.GetAllAsync(page, pageSize);
        return Ok(new { data, total, page, pageSize });
    }

    [HttpGet("ano-letivo/{anoLetivoId}")]
    public async Task<IActionResult> GetByAnoLetivo(int anoLetivoId)
    {
        var data = await _service.GetByAnoLetivoAsync(anoLetivoId);
        return Ok(data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var turma = await _service.GetByIdAsync(id);
        if (turma == null) return NotFound();
        return Ok(turma);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] TurmaRequestDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] TurmaRequestDto dto)
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