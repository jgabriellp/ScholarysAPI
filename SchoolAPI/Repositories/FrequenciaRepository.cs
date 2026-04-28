using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models;
using SchoolAPI.Repositories.Interfaces;

namespace SchoolAPI.Repositories;

public class FrequenciaRepository : IFrequenciaRepository
{
    private readonly AppDbContext _context;

    public FrequenciaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Frequencia>> GetByTurmaAsync(int turmaId, int anoLetivoId)
        => await _context.Frequencias
            .Include(f => f.FrequenciaAlunos)
                .ThenInclude(fa => fa.Aluno)
            .Where(f => f.TurmaId == turmaId && f.AnoLetivoId == anoLetivoId)
            .OrderBy(f => f.Data)
            .ToListAsync();

    public async Task<Frequencia?> GetByTurmaEDataAsync(int turmaId, DateOnly data)
        => await _context.Frequencias
            .Include(f => f.FrequenciaAlunos)
                .ThenInclude(fa => fa.Aluno)
            .FirstOrDefaultAsync(f => f.TurmaId == turmaId && f.Data == data);

    public async Task<Frequencia?> GetByIdAsync(int id)
        => await _context.Frequencias
            .Include(f => f.FrequenciaAlunos)
                .ThenInclude(fa => fa.Aluno)
            .FirstOrDefaultAsync(f => f.Id == id);

    public async Task<Frequencia> CreateAsync(Frequencia frequencia)
    {
        _context.Frequencias.Add(frequencia);
        await _context.SaveChangesAsync();
        return (await GetByIdAsync(frequencia.Id))!;
    }

    public async Task<Frequencia> UpdateAsync(Frequencia frequencia)
    {
        _context.Frequencias.Update(frequencia);
        await _context.SaveChangesAsync();
        return (await GetByIdAsync(frequencia.Id))!;
    }
}