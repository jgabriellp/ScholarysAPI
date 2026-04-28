using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs.Frequencia;
using SchoolAPI.Services;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FrequenciaController : ControllerBase
{
    private readonly FrequenciaService _service;

    public FrequenciaController(FrequenciaService service)
    {
        _service = service;
    }

    [HttpGet("turma/{turmaId}/ano/{anoLetivoId}")]
    [Authorize(Roles = "Admin,Diretor,Coordenador,Professor")]
    public async Task<IActionResult> GetByTurma(int turmaId, int anoLetivoId)
    {
        var data = await _service.GetByTurmaAsync(turmaId, anoLetivoId);
        return Ok(data);
    }

    [HttpGet("turma/{turmaId}/data/{data}")]
    [Authorize(Roles = "Admin,Diretor,Coordenador,Professor")]
    public async Task<IActionResult> GetByTurmaEData(int turmaId, DateOnly data)
    {
        var result = await _service.GetByTurmaEDataAsync(turmaId, data);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Diretor,Coordenador,Professor")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Professor")]
    public async Task<IActionResult> Lancar([FromBody] FrequenciaRequestDto dto)
    {
        var result = await _service.LancarAsync(dto);
        return Ok(result);
    }

    [HttpGet("aluno/{alunoId}/turma/{turmaId}/ano/{anoLetivoId}")]
    public async Task<IActionResult> CalcularFrequencia(int alunoId, int turmaId, int anoLetivoId)
    {
        var frequencia = await _service.CalcularFrequenciaAsync(alunoId, turmaId, anoLetivoId);
        return Ok(new { alunoId, turmaId, anoLetivoId, frequencia });
    }
}