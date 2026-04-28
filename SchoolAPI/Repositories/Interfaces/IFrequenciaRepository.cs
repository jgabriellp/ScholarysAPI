using SchoolAPI.Models;

namespace SchoolAPI.Repositories.Interfaces;

public interface IFrequenciaRepository
{
    Task<IEnumerable<Frequencia>> GetByTurmaAsync(int turmaId, int anoLetivoId);
    Task<Frequencia?> GetByTurmaEDataAsync(int turmaId, DateOnly data);
    Task<Frequencia?> GetByIdAsync(int id);
    Task<Frequencia> CreateAsync(Frequencia frequencia);
    Task<Frequencia> UpdateAsync(Frequencia frequencia);
}