using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs.DiaLetivo;
using SchoolAPI.Models.Enum;
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
    public async Task<IActionResult> GetByAnoLetivo(int anoLetivoId, [FromQuery] SegmentoEnum? segmento = null)
    {
        var data = await _service.GetByAnoLetivoAsync(anoLetivoId, segmento);
        return Ok(data);
    }

    [HttpPost("lote")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateLote([FromBody] DiaLetivoLoteRequestDto dto)
    {
        try
        {
            var criados = await _service.CreateLoteAsync(dto);
            return Ok(criados);
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
