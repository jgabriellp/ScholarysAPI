using SchoolAPI.Models;

namespace SchoolAPI.Repositories.Interfaces;

public interface IRelatoAulaRepository
{
    Task<IEnumerable<RelatoAula>> GetByTurmaEAnoAsync(int turmaId, int anoLetivoId);
    Task<RelatoAula?> GetByDiaETurmaEProfessorAsync(int diaLetivoId, int turmaId, int professorId);
    Task<RelatoAula?> GetByIdAsync(int id);
    Task<RelatoAula> CreateAsync(RelatoAula relato);
    Task<RelatoAula> UpdateAsync(RelatoAula relato);
}
