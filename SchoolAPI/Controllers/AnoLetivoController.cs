using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs.AnoLetivo;
using SchoolAPI.Services;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AnoLetivoController : ControllerBase
{
    private readonly AnoLetivoService _service;

    public AnoLetivoController(AnoLetivoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var (data, total) = await _service.GetAllAsync(page, pageSize);
        return Ok(new { data, total, page, pageSize });
    }

    [HttpGet("ativo")]
    public async Task<IActionResult> GetAtivo()
    {
        var ano = await _service.GetAtivoAsync();
        if (ano == null) return NotFound();
        return Ok(ano);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ano = await _service.GetByIdAsync(id);
        if (ano == null) return NotFound();
        return Ok(ano);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AnoLetivoRequestDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] AnoLetivoRequestDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}