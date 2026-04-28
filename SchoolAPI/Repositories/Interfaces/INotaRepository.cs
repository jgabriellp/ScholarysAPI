using SchoolAPI.Models;

namespace SchoolAPI.Repositories.Interfaces;

public interface INotaRepository
{
    Task<IEnumerable<Nota>> GetByAlunoAsync(int alunoId, int anoLetivoId);
    Task<IEnumerable<Nota>> GetByTurmaEDisciplinaAsync(int turmaId, int disciplinaId, int anoLetivoId);
    Task<Nota?> GetByIdAsync(int id);
    Task<Nota?> GetByAlunoUnidadeAsync(int alunoId, int disciplinaId, int unidade, int anoLetivoId);
    Task<Nota> CreateAsync(Nota nota);
    Task<Nota> UpdateAsync(Nota nota);
    Task DeleteAsync(Nota nota);
}