using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models;
using SchoolAPI.Repositories.Interfaces;

namespace SchoolAPI.Repositories;

public class TurmaDisciplinaProfessorRepository : ITurmaDisciplinaProfessorRepository
{
    private readonly AppDbContext _context;

    public TurmaDisciplinaProfessorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TurmaDisciplinaProfessor>> GetByTurmaAsync(int turmaId)
        => await _context.TurmaDisciplinaProfessores
            .Include(t => t.Turma)
            .Include(t => t.Disciplina)
            .Include(t => t.Professor)
            .Include(t => t.AnoLetivo)
            .Where(t => t.TurmaId == turmaId)
            .ToListAsync();

    public async Task<IEnumerable<TurmaDisciplinaProfessor>> GetByProfessorAsync(int professorId)
        => await _context.TurmaDisciplinaProfessores
            .Include(t => t.Turma)
            .Include(t => t.Disciplina)
            .Include(t => t.Professor)
            .Include(t => t.AnoLetivo)
            .Where(t => t.ProfessorId == professorId)
            .ToListAsync();

    public async Task<TurmaDisciplinaProfessor?> GetByIdAsync(int id)
        => await _context.TurmaDisciplinaProfessores
            .Include(t => t.Turma)
            .Include(t => t.Disciplina)
            .Include(t => t.Professor)
            .Include(t => t.AnoLetivo)
            .FirstOrDefaultAsync(t => t.Id == id);

    public async Task<TurmaDisciplinaProfessor> CreateAsync(TurmaDisciplinaProfessor turmaDisciplinaProfessor)
    {
        _context.TurmaDisciplinaProfessores.Add(turmaDisciplinaProfessor);
        await _context.SaveChangesAsync();
        return (await GetByIdAsync(turmaDisciplinaProfessor.Id))!;
    }

    public async Task DeleteAsync(TurmaDisciplinaProfessor turmaDisciplinaProfessor)
    {
        _context.TurmaDisciplinaProfessores.Remove(turmaDisciplinaProfessor);
        await _context.SaveChangesAsync();
    }
}