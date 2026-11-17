using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Helpers;
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

    // Um mesmo usuário pode ter um registro de Aluno por ano letivo (renovação de matrícula).
    // Sem anoLetivoId, devolve o registro do ano corrente (ver Helpers/AnoCorrente).
    public async Task<Aluno?> GetByUsuarioAsync(int userId, int? anoLetivoId = null)
    {
        var query = _context.Alunos
            .Include(a => a.Turma)
            .Include(a => a.AnoLetivo)
            .Include(a => a.User)
            .Where(a => a.UserId == userId && a.Ativo);

        if (anoLetivoId.HasValue)
            return await query.FirstOrDefaultAsync(a => a.AnoLetivoId == anoLetivoId.Value);

        var anoAtual = AnoCorrente.AnoAtual();
        return await query
            .OrderBy(a => a.AnoLetivo.Ano <= anoAtual ? anoAtual - a.AnoLetivo.Ano : a.AnoLetivo.Ano)
            .FirstOrDefaultAsync();
    }

    // Considera também registros excluídos (soft delete), pois o índice único (UserId, AnoLetivoId) os inclui.
    public async Task<bool> ExisteParaUsuarioNoAnoAsync(int userId, int anoLetivoId, int? ignorarAlunoId = null)
        => await _context.Alunos.AnyAsync(a =>
            a.UserId == userId &&
            a.AnoLetivoId == anoLetivoId &&
            (ignorarAlunoId == null || a.Id != ignorarAlunoId));

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