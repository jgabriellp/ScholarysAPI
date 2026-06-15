using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs.DiaLetivo;
using SchoolAPI.Services;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DiaLetivoController : ControllerBase
{
    private readonly DiaLetivoService _service;

    public DiaLetivoController(DiaLetivoService service)
    {
        _service = service;
    }

    [HttpGet("ano-letivo/{anoLetivoId}")]
    public async Task<IActionResult> GetByAnoLetivo(int anoLetivoId)
    {
        var data = await _service.GetByAnoLetivoAsync(anoLetivoId);
        return Ok(data);
    }

    [HttpPost("lote")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateLote([FromBody] DiaLetivoLoteRequestDto dto)
    {
        var criados = await _service.CreateLoteAsync(dto);
        return Ok(criados);
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
