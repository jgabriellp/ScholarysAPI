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

    public async Task<DashboardResponseDto> GetDashboardInfoAsync()
    {
        var turmasCount = await _context.Turmas.CountAsync(t => t.AnoLetivo.Ativo == true);
        var alunosCount = await _context.Alunos.CountAsync(a => a.AnoLetivo.Ativo == true);
        var disciplinasCount = await _context.Disciplinas.CountAsync(d => d.Ativo);
        var professoresCount = await _context.Users.CountAsync(u => u.Ativo && u.Role == RoleEnum.Professor);

        return new DashboardResponseDto(turmasCount, alunosCount, disciplinasCount, professoresCount);
    }
}
    