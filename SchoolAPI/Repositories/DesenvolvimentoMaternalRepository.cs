using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models;
using SchoolAPI.Repositories.Interfaces;

namespace SchoolAPI.Repositories;

public class DesenvolvimentoMaternalRepository : IDesenvolvimentoMaternalRepository
{
    private readonly AppDbContext _context;

    public DesenvolvimentoMaternalRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DesenvolvimentoMaternal>> GetByAlunoAsync(int alunoId, int anoLetivoId)
        => await _context.DesenvolvimentosMaternal
            .Include(d => d.Aluno)
            .Include(d => d.Turma)
            .Where(d => d.AlunoId == alunoId && d.AnoLetivoId == anoLetivoId)
            .OrderBy(d => d.Bimestre)
            .ToListAsync();

    public async Task<DesenvolvimentoMaternal?> GetByAlunoEBimestreAsync(int alunoId, int bimestre, int anoLetivoId)
        => await _context.DesenvolvimentosMaternal
            .Include(d => d.Aluno)
            .Include(d => d.Turma)
            .FirstOrDefaultAsync(d =>
                d.AlunoId == alunoId &&
                d.Bimestre == bimestre &&
                d.AnoLetivoId == anoLetivoId);

    public async Task<DesenvolvimentoMaternal?> GetByIdAsync(int id)
        => await _context.DesenvolvimentosMaternal
            .Include(d => d.Aluno)
            .Include(d => d.Turma)
            .FirstOrDefaultAsync(d => d.Id == id);

    public async Task<DesenvolvimentoMaternal> CreateAsync(DesenvolvimentoMaternal desenvolvimento)
    {
        _context.DesenvolvimentosMaternal.Add(desenvolvimento);
        await _context.SaveChangesAsync();
        return (await GetByIdAsync(desenvolvimento.Id))!;
    }

    public async Task<DesenvolvimentoMaternal> UpdateAsync(DesenvolvimentoMaternal desenvolvimento)
    {
        _context.DesenvolvimentosMaternal.Update(desenvolvimento);
        await _context.SaveChangesAsync();
        return (await GetByIdAsync(desenvolvimento.Id))!;
    }

    public async Task DeleteAsync(DesenvolvimentoMaternal desenvolvimento)
    {
        _context.DesenvolvimentosMaternal.Remove(desenvolvimento);
        await _context.SaveChangesAsync();
    }
}