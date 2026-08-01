using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models;
using SchoolAPI.Models.Enum;
using SchoolAPI.Repositories.Interfaces;

namespace SchoolAPI.Repositories;

public class DiaLetivoRepository : IDiaLetivoRepository
{
    private readonly AppDbContext _context;

    public DiaLetivoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DiaLetivo>> GetByAnoLetivoAsync(int anoLetivoId, SegmentoEnum? segmento = null)
    {
        var query = _context.DiasLetivos
            .Include(d => d.AnoLetivo)
            .Where(d => d.AnoLetivoId == anoLetivoId);

        if (segmento.HasValue)
            query = query.Where(d => d.Segmento == segmento.Value);

        return await query.OrderBy(d => d.Data).ToListAsync();
    }

    public async Task<DiaLetivo?> GetByIdAsync(int id)
        => await _context.DiasLetivos
            .Include(d => d.AnoLetivo)
            .FirstOrDefaultAsync(d => d.Id == id);

    public async Task<IEnumerable<DiaLetivo>> CreateLoteAsync(IEnumerable<DiaLetivo> dias)
    {
        var lista = dias.ToList();
        if (lista.Count == 0) return [];

        var anoLetivoId = lista[0].AnoLetivoId;
        var segmento = lista[0].Segmento;

        var datasExistentes = await _context.DiasLetivos
            .Where(d => d.AnoLetivoId == anoLetivoId && d.Segmento == segmento)
            .Select(d => d.Data)
            .ToListAsync();

        var novas = lista.Where(d => !datasExistentes.Contains(d.Data)).ToList();
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
