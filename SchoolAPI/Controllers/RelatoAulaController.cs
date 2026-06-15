using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs.RelatoAula;
using SchoolAPI.Services;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RelatoAulaController : ControllerBase
{
    private readonly RelatoAulaService _service;

    public RelatoAulaController(RelatoAulaService service)
    {
        _service = service;
    }

    [HttpGet("turma/{turmaId}/ano/{anoLetivoId}")]
    public async Task<IActionResult> GetByTurmaEAno(int turmaId, int anoLetivoId)
    {
        var data = await _service.GetByTurmaEAnoAsync(turmaId, anoLetivoId);
        return Ok(data);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Professor")]
    public async Task<IActionResult> Upsert([FromBody] RelatoAulaRequestDto dto)
    {
        var result = await _service.UpsertAsync(dto);
        return Ok(result);
    }
}
