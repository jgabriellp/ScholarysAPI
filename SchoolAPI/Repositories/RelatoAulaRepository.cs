using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models;
using SchoolAPI.Repositories.Interfaces;

namespace SchoolAPI.Repositories;

public class RelatoAulaRepository : IRelatoAulaRepository
{
    private readonly AppDbContext _context;

    public RelatoAulaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RelatoAula>> GetByTurmaEAnoAsync(int turmaId, int anoLetivoId)
        => await _context.RelatosAula
            .Include(r => r.DiaLetivo)
            .Include(r => r.Turma)
            .Include(r => r.Professor)
            .Where(r => r.TurmaId == turmaId && r.DiaLetivo.AnoLetivoId == anoLetivoId)
            .OrderBy(r => r.DiaLetivo.Data)
            .ToListAsync();

    public async Task<RelatoAula?> GetByDiaETurmaEProfessorAsync(int diaLetivoId, int turmaId, int professorId)
        => await _context.RelatosAula
            .Include(r => r.DiaLetivo)
            .Include(r => r.Turma)
            .Include(r => r.Professor)
            .FirstOrDefaultAsync(r => r.DiaLetivoId == diaLetivoId && r.TurmaId == turmaId && r.ProfessorId == professorId);

    public async Task<RelatoAula?> GetByIdAsync(int id)
        => await _context.RelatosAula
            .Include(r => r.DiaLetivo)
            .Include(r => r.Turma)
            .Include(r => r.Professor)
            .FirstOrDefaultAsync(r => r.Id == id);

    public async Task<RelatoAula> CreateAsync(RelatoAula relato)
    {
        _context.RelatosAula.Add(relato);
        await _context.SaveChangesAsync();
        return (await GetByIdAsync(relato.Id))!;
    }

    public async Task<RelatoAula> UpdateAsync(RelatoAula relato)
    {
        _context.RelatosAula.Update(relato);
        await _context.SaveChangesAsync();
        return (await GetByIdAsync(relato.Id))!;
    }
}
