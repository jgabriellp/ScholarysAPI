using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.DTOs.Dashboard;
using SchoolAPI.Models.Enum;
using SchoolAPI.Repositories.Interfaces;

namespace SchoolAPI.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly AppDbContext _context;

    public DashboardRepository(AppDbContext context)
    {
        _context = context;
    }

    // Contagens de turmas/alunos restritas a um ano letivo (antes somava todos os anos cadastrados).
    public async Task<DashboardResponseDto> GetDashboardInfoAsync(int? anoLetivoId)
    {
        var turmasCount = anoLetivoId == null ? 0
            : await _context.Turmas.CountAsync(t => t.AnoLetivoId == anoLetivoId);
        var alunosCount = anoLetivoId == null ? 0
            : await _context.Alunos.CountAsync(a => a.AnoLetivoId == anoLetivoId);
        var disciplinasCount = await _context.Disciplinas.CountAsync(d => d.Ativo);
        var professoresCount = await _context.Users.CountAsync(u => u.Ativo && u.Role == RoleEnum.Professor);

        return new DashboardResponseDto(turmasCount, alunosCount, disciplinasCount, professoresCount);
    }
}
    