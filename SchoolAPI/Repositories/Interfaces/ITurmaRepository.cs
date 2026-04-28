using SchoolAPI.Models;

namespace SchoolAPI.Repositories.Interfaces;

public interface ITurmaRepository
{
    Task<(IEnumerable<Turma> Data, int Total)> GetAllAsync(int page, int pageSize);
    Task<IEnumerable<Turma>> GetByAnoLetivoAsync(int anoLetivoId);
    Task<Turma?> GetByIdAsync(int id);
    Task<Turma> CreateAsync(Turma turma);
    Task<Turma> UpdateAsync(Turma turma);
    Task DeleteAsync(Turma turma);
}