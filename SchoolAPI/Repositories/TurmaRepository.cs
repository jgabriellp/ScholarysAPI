using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models;
using SchoolAPI.Repositories.Interfaces;

namespace SchoolAPI.Repositories;

public class TurmaRepository : ITurmaRepository
{
    private readonly AppDbContext _context;

    public TurmaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<Turma> Data, int Total)> GetAllAsync(int page, int pageSize)
    {
        var query = _context.Turmas
            .Include(t => t.AnoLetivo)
            .Where(t => t.Ativo);

        var total = await query.CountAsync();
        var data = await query
            .OrderBy(t => t.Nome)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (data, total);
    }

    public async Task<IEnumerable<Turma>> GetByAnoLetivoAsync(int anoLetivoId)
        => await _context.Turmas
            .Include(t => t.AnoLetivo)
            .Where(t => t.AnoLetivoId == anoLetivoId && t.Ativo)
            .OrderBy(t => t.Nome)
            .ToListAsync();

    public async Task<Turma?> GetByIdAsync(int id)
        => await _context.Turmas
            .Include(t => t.AnoLetivo)
            .FirstOrDefaultAsync(t => t.Id == id && t.Ativo);

    public async Task<Turma> CreateAsync(Turma turma)
    {
        _context.Turmas.Add(turma);
        await _context.SaveChangesAsync();
        return (await GetByIdAsync(turma.Id))!;
    }

    public async Task<Turma> UpdateAsync(Turma turma)
    {
        _context.Turmas.Update(turma);
        await _context.SaveChangesAsync();
        return (await GetByIdAsync(turma.Id))!;
    }

    public async Task DeleteAsync(Turma turma)
    {
        turma.Ativo = false;
        _context.Turmas.Update(turma);
        await _context.SaveChangesAsync();
    }
}