using SchoolAPI.DTOs.Aluno;
using SchoolAPI.Models;
using SchoolAPI.Repositories.Interfaces;

namespace SchoolAPI.Services;

public class AlunoService
{
    private readonly IAlunoRepository _repository;

    public AlunoService(IAlunoRepository repository)
    {
        _repository = repository;
    }

    public async Task<(IEnumerable<AlunoResponseDto> Data, int Total)> GetAllAsync(int page, int pageSize)
    {
        var (alunos, total) = await _repository.GetAllAsync(page, pageSize);
        var data = alunos.Select(Map);
        return (data, total);
    }

    public async Task<IEnumerable<AlunoResponseDto>> GetByTurmaAsync(int turmaId)
    {
        var alunos = await _repository.GetByTurmaAsync(turmaId);
        return alunos.Select(Map);
    }

    public async Task<AlunoResponseDto?> GetByIdAsync(int id)
    {
        var aluno = await _repository.GetByIdAsync(id);
        if (aluno == null) return null;
        return Map(aluno);
    }

    public async Task<AlunoResponseDto> CreateAsync(AlunoRequestDto dto)
    {
        var aluno = new Aluno
        {
            Nome = dto.Nome,
            NumeroChamada = dto.NumeroChamada,
            DataNascimento = dto.DataNascimento,
            TurmaId = dto.TurmaId,
            AnoLetivoId = dto.AnoLetivoId,
            UserId = dto.UserId,
            Ativo = true
        };

        var created = await _repository.CreateAsync(aluno);
        return Map(created);
    }

    public async Task<AlunoResponseDto?> UpdateAsync(int id, AlunoRequestDto dto)
    {
        var aluno = await _repository.GetByIdAsync(id);
        if (aluno == null) return null;

        aluno.Nome = dto.Nome;
        aluno.NumeroChamada = dto.NumeroChamada;
        aluno.DataNascimento = dto.DataNascimento;
        aluno.TurmaId = dto.TurmaId;
        aluno.AnoLetivoId = dto.AnoLetivoId;
        aluno.UserId = dto.UserId;

        var updated = await _repository.UpdateAsync(aluno);
        return Map(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var aluno = await _repository.GetByIdAsync(id);
        if (aluno == null) return false;

        await _repository.DeleteAsync(aluno);
        return true;
    }

    private static AlunoResponseDto Map(Aluno a) => new(
        a.Id,
        a.Nome,
        a.NumeroChamada,
        a.DataNascimento,
        a.TurmaId,
        a.Turma?.Nome ?? "",
        a.AnoLetivoId,
        a.AnoLetivo?.Ano ?? 0,
        a.UserId,
        a.Ativo
    );
}