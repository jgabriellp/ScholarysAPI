using SchoolAPI.Models;

namespace SchoolAPI.Repositories.Interfaces;

public interface IDisciplinaRepository
{
    Task<(IEnumerable<Disciplina> Data, int Total)> GetAllAsync(int page, int pageSize);
    Task<Disciplina?> GetByIdAsync(int id);
    Task<Disciplina> CreateAsync(Disciplina disciplina);
    Task<Disciplina> UpdateAsync(Disciplina disciplina);
    Task DeleteAsync(Disciplina disciplina);
}