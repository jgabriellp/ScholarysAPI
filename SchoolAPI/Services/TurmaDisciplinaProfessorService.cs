using SchoolAPI.DTOs.TurmaDisciplinaProfessor;
using SchoolAPI.Models;
using SchoolAPI.Repositories.Interfaces;

namespace SchoolAPI.Services;

public class TurmaDisciplinaProfessorService
{
    private readonly ITurmaDisciplinaProfessorRepository _repository;

    public TurmaDisciplinaProfessorService(ITurmaDisciplinaProfessorRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TurmaDisciplinaProfessorResponseDto>> GetByTurmaAsync(int turmaId)
    {
        var data = await _repository.GetByTurmaAsync(turmaId);
        return data.Select(Map);
    }

    public async Task<IEnumerable<TurmaDisciplinaProfessorResponseDto>> GetByProfessorAsync(int professorId)
    {
        var data = await _repository.GetByProfessorAsync(professorId);
        return data.Select(Map);
    }

    public async Task<TurmaDisciplinaProfessorResponseDto?> GetByIdAsync(int id)
    {
        var data = await _repository.GetByIdAsync(id);
        if (data == null) return null;
        return Map(data);
    }

    public async Task<TurmaDisciplinaProfessorResponseDto> CreateAsync(TurmaDisciplinaProfessorRequestDto dto)
    {
        var entity = new TurmaDisciplinaProfessor
        {
            TurmaId = dto.TurmaId,
            DisciplinaId = dto.DisciplinaId,
            ProfessorId = dto.ProfessorId,
            AnoLetivoId = dto.AnoLetivoId
        };

        var created = await _repository.CreateAsync(entity);
        return Map(created);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null) return false;

        await _repository.DeleteAsync(entity);
        return true;
    }

    private static TurmaDisciplinaProfessorResponseDto Map(TurmaDisciplinaProfessor t) => new(
        t.Id,
        t.TurmaId,
        t.Turma?.Nome ?? "",
        t.DisciplinaId,
        t.Disciplina?.Nome ?? "",
        t.ProfessorId,
        t.Professor?.Nome ?? "",
        t.AnoLetivoId,
        t.AnoLetivo?.Ano ?? 0
    );
}