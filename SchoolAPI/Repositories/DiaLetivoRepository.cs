using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models;
using SchoolAPI.Repositories.Interfaces;

namespace SchoolAPI.Repositories;

public class DiaLetivoRepository : IDiaLetivoRepository
{
    private readonly AppDbContext _context;

    public DiaLetivoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DiaLetivo>> GetByAnoLetivoAsync(int anoLetivoId)
        => await _context.DiasLetivos
            .Include(d => d.AnoLetivo)
            .Where(d => d.AnoLetivoId == anoLetivoId)
            .OrderBy(d => d.Data)
            .ToListAsync();

    public async Task<DiaLetivo?> GetByIdAsync(int id)
        => await _context.DiasLetivos
            .Include(d => d.AnoLetivo)
            .FirstOrDefaultAsync(d => d.Id == id);

    public async Task<IEnumerable<DiaLetivo>> CreateLoteAsync(IEnumerable<DiaLetivo> dias)
    {
        var datasExistentes = await _context.DiasLetivos
            .Where(d => d.AnoLetivoId == dias.First().AnoLetivoId)
            .Select(d => d.Data)
            .ToListAsync();

        var novas = dias.Where(d => !datasExistentes.Contains(d.Data)).ToList();
        if (novas.Count == 0) return [];

        _context.DiasLetivos.AddRange(novas);
        await _context.SaveChangesAsync();

        return novas;
    }

    public async Task DeleteAsync(DiaLetivo dia)
    {
        _context.DiasLetivos.Remove(dia);
        await _context.SaveChangesAsync();
    }
}
