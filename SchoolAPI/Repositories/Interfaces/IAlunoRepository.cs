using SchoolAPI.Models;

namespace SchoolAPI.Repositories.Interfaces;

public interface IAlunoRepository
{
    Task<(IEnumerable<Aluno> Data, int Total)> GetAllAsync(int page, int pageSize);
    Task<IEnumerable<Aluno>> GetByTurmaAsync(int turmaId);
    Task<Aluno?> GetByIdAsync(int id);
    Task<Aluno> CreateAsync(Aluno aluno);
    Task<Aluno> UpdateAsync(Aluno aluno);
    Task DeleteAsync(Aluno aluno);
}