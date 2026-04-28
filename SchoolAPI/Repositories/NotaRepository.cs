using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models;
using SchoolAPI.Repositories.Interfaces;

namespace SchoolAPI.Repositories;

public class NotaRepository : INotaRepository
{
    private readonly AppDbContext _context;

    public NotaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Nota>> GetByAlunoAsync(int alunoId, int anoLetivoId)
        => await _context.Notas
            .Include(n => n.Aluno)
            .Include(n => n.Disciplina)
            .Where(n => n.AlunoId == alunoId && n.AnoLetivoId == anoLetivoId)
            .OrderBy(n => n.DisciplinaId)
            .ThenBy(n => n.Unidade)
            .ToListAsync();

    public async Task<IEnumerable<Nota>> GetByTurmaEDisciplinaAsync(int turmaId, int disciplinaId, int anoLetivoId)
        => await _context.Notas
            .Include(n => n.Aluno)
            .Include(n => n.Disciplina)
            .Where(n => n.TurmaId == turmaId && n.DisciplinaId == disciplinaId && n.AnoLetivoId == anoLetivoId)
            .OrderBy(n => n.Aluno.NumeroChamada)
            .ThenBy(n => n.Unidade)
            .ToListAsync();

    public async Task<Nota?> GetByIdAsync(int id)
        => await _context.Notas
            .Include(n => n.Disciplina)
            .Include(n => n.Aluno)
            .FirstOrDefaultAsync(n => n.Id == id);

    public async Task<Nota?> GetByAlunoUnidadeAsync(int alunoId, int disciplinaId, int unidade, int anoLetivoId)
        => await _context.Notas
            .FirstOrDefaultAsync(n =>
                n.AlunoId == alunoId &&
                n.DisciplinaId == disciplinaId &&
                n.Unidade == unidade &&
                n.AnoLetivoId == anoLetivoId);

    public async Task<Nota> CreateAsync(Nota nota)
    {
        _context.Notas.Add(nota);
        await _context.SaveChangesAsync();
        return (await GetByIdAsync(nota.Id))!;
    }

    public async Task<Nota> UpdateAsync(Nota nota)
    {
        _context.Notas.Update(nota);
        await _context.SaveChangesAsync();
        return (await GetByIdAsync(nota.Id))!;
    }

    public async Task DeleteAsync(Nota nota)
    {
        _context.Notas.Remove(nota);
        await _context.SaveChangesAsync();
    }
}