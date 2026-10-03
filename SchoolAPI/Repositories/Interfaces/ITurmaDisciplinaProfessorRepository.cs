using SchoolAPI.Models;

namespace SchoolAPI.Repositories.Interfaces;

public interface ITurmaDisciplinaProfessorRepository
{
    Task<IEnumerable<TurmaDisciplinaProfessor>> GetByTurmaAsync(int turmaId);
    Task<IEnumerable<TurmaDisciplinaProfessor>> GetByProfessorAsync(int professorId);
    Task<IEnumerable<TurmaDisciplinaProfessor>> GetByTurmaEProfessorAsync(int turmaId, int professorId, int anoLetivoId);
    Task<TurmaDisciplinaProfessor?> GetByIdAsync(int id);
    Task<TurmaDisciplinaProfessor> CreateAsync(TurmaDisciplinaProfessor turmaDisciplinaProfessor);
    Task<TurmaDisciplinaProfessor> UpdateAsync(TurmaDisciplinaProfessor turmaDisciplinaProfessor);
    Task DeleteAsync(TurmaDisciplinaProfessor turmaDisciplinaProfessor);
}