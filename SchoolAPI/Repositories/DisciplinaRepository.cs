using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models;
using SchoolAPI.Repositories.Interfaces;

namespace SchoolAPI.Repositories;

public class DisciplinaRepository : IDisciplinaRepository
{
    private readonly AppDbContext _context;

    public DisciplinaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<Disciplina> Data, int Total)> GetAllAsync(int page, int pageSize)
    {
        var query = _context.Disciplinas.Where(d => d.Ativo);
        var total = await query.CountAsync();
        var data = await query
            .OrderBy(d => d.Nome)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (data, total);
    }

    public async Task<Disciplina?> GetByIdAsync(int id)
        => await _context.Disciplinas.FirstOrDefaultAsync(d => d.Id == id && d.Ativo);

    public async Task<Disciplina> CreateAsync(Disciplina disciplina)
    {
        _context.Disciplinas.Add(disciplina);
        await _context.SaveChangesAsync();
        return disciplina;
    }

    public async Task<Disciplina> UpdateAsync(Disciplina disciplina)
    {
        _context.Disciplinas.Update(disciplina);
        await _context.SaveChangesAsync();
        return disciplina;
    }

    public async Task DeleteAsync(Disciplina disciplina)
    {
        disciplina.Ativo = false;
        _context.Disciplinas.Update(disciplina);
        await _context.SaveChangesAsync();
    }
}