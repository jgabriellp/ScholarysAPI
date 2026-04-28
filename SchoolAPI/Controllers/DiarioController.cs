using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Services;

namespace SchoolAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DiarioController : ControllerBase
{
    private readonly DiarioService _service;

    public DiarioController(DiarioService service)
    {
        _service = service;
    }

    [HttpGet("maternal/aluno/{alunoId}/ano/{anoLetivoId}")]
    public async Task<IActionResult> GetDiarioMaternal(int alunoId, int anoLetivoId)
    {
        var diario = await _service.GetDiarioMaternalAsync(alunoId, anoLetivoId);
        if (diario == null) return NotFound();
        return Ok(diario);
    }

    [HttpGet("fundamental/aluno/{alunoId}/ano/{anoLetivoId}")]
    public async Task<IActionResult> GetDiarioFundamental(int alunoId, int anoLetivoId)
    {
        var diario = await _service.GetDiarioFundamentalAsync(alunoId, anoLetivoId);
        if (diario == null) return NotFound();
        return Ok(diario);
    }
}