using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models;
using SchoolAPI.Repositories.Interfaces;

namespace SchoolAPI.Repositories;

public class AlunoRepository : IAlunoRepository
{
    private readonly AppDbContext _context;

    public AlunoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<Aluno> Data, int Total)> GetAllAsync(int page, int pageSize)
    {
        var query = _context.Alunos
            .Include(a => a.Turma)
            .Include(a => a.AnoLetivo)
            .Include(a => a.User)
            .Where(a => a.Ativo);

        var total = await query.CountAsync();
        var data = await query
            .OrderBy(a => a.Nome)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (data, total);
    }

    public async Task<IEnumerable<Aluno>> GetByTurmaAsync(int turmaId)
        => await _context.Alunos
            .Include(a => a.Turma)
            .Include(a => a.AnoLetivo)
            .Include(a => a.User)
            .Where(a => a.TurmaId == turmaId && a.Ativo)
            .OrderBy(a => a.NumeroChamada)
            .ToListAsync();

    public async Task<Aluno?> GetByIdAsync(int id)
        => await _context.Alunos
            .Include(a => a.Turma)
            .Include(a => a.AnoLetivo)
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == id && a.Ativo);

    public async Task<Aluno> CreateAsync(Aluno aluno)
    {
        _context.Alunos.Add(aluno);
        await _context.SaveChangesAsync();
        return (await GetByIdAsync(aluno.Id))!;
    }

    public async Task<Aluno> UpdateAsync(Aluno aluno)
    {
        _context.Alunos.Update(aluno);
        await _context.SaveChangesAsync();
        return (await GetByIdAsync(aluno.Id))!;
    }

    public async Task DeleteAsync(Aluno aluno)
    {
        aluno.Ativo = false;
        _context.Alunos.Update(aluno);
        await _context.SaveChangesAsync();
    }
}