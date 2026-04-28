using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models;
using SchoolAPI.Repositories.Interfaces;

namespace SchoolAPI.Repositories;

public class AnoLetivoRepository : IAnoLetivoRepository
{
    private readonly AppDbContext _context;

    public AnoLetivoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<AnoLetivo> Data, int Total)> GetAllAsync(int page, int pageSize)
    {
        var query = _context.AnosLetivos.Where(a => a.Ativo);
        var total = await query.CountAsync();
        var data = await query
            .OrderByDescending(a => a.Ano)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (data, total);
    }

    public async Task<AnoLetivo?> GetByIdAsync(int id)
        => await _context.AnosLetivos.FirstOrDefaultAsync(a => a.Id == id && a.Ativo);

    public async Task<AnoLetivo?> GetAtivoAsync()
        => await _context.AnosLetivos.FirstOrDefaultAsync(a => a.Ativo);

    public async Task<AnoLetivo> CreateAsync(AnoLetivo anoLetivo)
    {
        _context.AnosLetivos.Add(anoLetivo);
        await _context.SaveChangesAsync();
        return anoLetivo;
    }

    public async Task<AnoLetivo> UpdateAsync(AnoLetivo anoLetivo)
    {
        _context.AnosLetivos.Update(anoLetivo);
        await _context.SaveChangesAsync();
        return anoLetivo;
    }

    public async Task DeleteAsync(AnoLetivo anoLetivo)
    {
        anoLetivo.Ativo = false;
        _context.AnosLetivos.Update(anoLetivo);
        await _context.SaveChangesAsync();
    }
}