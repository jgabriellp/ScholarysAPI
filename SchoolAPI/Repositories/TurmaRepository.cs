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

    public async Task<IEnumerable<Turma>> GetByProfessorAsync(int professorId, int? anoLetivoId)
    {
        var query = _context.TurmaDisciplinaProfessores
            .Include(tdp => tdp.Turma)
                .ThenInclude(t => t!.AnoLetivo)
            .Where(tdp => tdp.ProfessorId == professorId && tdp.Turma!.Ativo);

        if (anoLetivoId.HasValue)
            query = query.Where(tdp => tdp.AnoLetivoId == anoLetivoId.Value);

        var turmas = await query
            .Select(tdp => tdp.Turma!)
            .Distinct()
            .OrderBy(t => t.Nome)
            .ToListAsync();

        return turmas;
    }

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
        var alunos = await _context.Alunos
            .Where(a => a.TurmaId == turma.Id && a.Ativo)
            .ToListAsync();
        foreach (var aluno in alunos)
            aluno.Ativo = false;
        _context.Alunos.UpdateRange(alunos);

        var vinculos = await _context.TurmaDisciplinaProfessores
            .Where(tdp => tdp.TurmaId == turma.Id)
            .ToListAsync();
        _context.TurmaDisciplinaProfessores.RemoveRange(vinculos);

        turma.Ativo = false;
        _context.Turmas.Update(turma);
        await _context.SaveChangesAsync();
    }
}