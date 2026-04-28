using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs.DesenvolvimentoMaternal;
using SchoolAPI.Services;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DesenvolvimentoMaternalController : ControllerBase
{
    private readonly DesenvolvimentoMaternalService _service;

    public DesenvolvimentoMaternalController(DesenvolvimentoMaternalService service)
    {
        _service = service;
    }

    [HttpGet("aluno/{alunoId}/ano/{anoLetivoId}")]
    public async Task<IActionResult> GetByAluno(int alunoId, int anoLetivoId)
    {
        var data = await _service.GetByAlunoAsync(alunoId, anoLetivoId);
        return Ok(data);
    }

    [HttpGet("aluno/{alunoId}/bimestre/{bimestre}/ano/{anoLetivoId}")]
    public async Task<IActionResult> GetByAlunoEBimestre(int alunoId, int bimestre, int anoLetivoId)
    {
        var data = await _service.GetByAlunoEBimestreAsync(alunoId, bimestre, anoLetivoId);
        if (data == null) return NotFound();
        return Ok(data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var data = await _service.GetByIdAsync(id);
        if (data == null) return NotFound();
        return Ok(data);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Professor")]
    public async Task<IActionResult> Salvar([FromBody] DesenvolvimentoMaternalRequestDto dto)
    {
        try
        {
            var result = await _service.SalvarAsync(dto);
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
}