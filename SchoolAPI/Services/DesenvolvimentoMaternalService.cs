using SchoolAPI.DTOs.DesenvolvimentoMaternal;
using SchoolAPI.Models;
using SchoolAPI.Repositories.Interfaces;

namespace SchoolAPI.Services;

public class DesenvolvimentoMaternalService
{
    private readonly IDesenvolvimentoMaternalRepository _repository;

    public DesenvolvimentoMaternalService(IDesenvolvimentoMaternalRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DesenvolvimentoMaternalResponseDto>> GetByAlunoAsync(int alunoId, int anoLetivoId)
    {
        var data = await _repository.GetByAlunoAsync(alunoId, anoLetivoId);
        return data.Select(Map);
    }

    public async Task<DesenvolvimentoMaternalResponseDto?> GetByAlunoEBimestreAsync(int alunoId, int bimestre, int anoLetivoId)
    {
        var data = await _repository.GetByAlunoEBimestreAsync(alunoId, bimestre, anoLetivoId);
        if (data == null) return null;
        return Map(data);
    }

    public async Task<DesenvolvimentoMaternalResponseDto?> GetByIdAsync(int id)
    {
        var data = await _repository.GetByIdAsync(id);
        if (data == null) return null;
        return Map(data);
    }

    public async Task<DesenvolvimentoMaternalResponseDto> SalvarAsync(DesenvolvimentoMaternalRequestDto dto)
    {
        if (dto.Bimestre < 1 || dto.Bimestre > 4)
            throw new ArgumentException("Bimestre deve ser entre 1 e 4.");

        var existente = await _repository.GetByAlunoEBimestreAsync(dto.AlunoId, dto.Bimestre, dto.AnoLetivoId);

        if (existente != null)
        {
            existente.Descricao = dto.Descricao;
            var updated = await _repository.UpdateAsync(existente);
            return Map(updated);
        }

        var desenvolvimento = new DesenvolvimentoMaternal
        {
            AlunoId = dto.AlunoId,
            TurmaId = dto.TurmaId,
            AnoLetivoId = dto.AnoLetivoId,
            Bimestre = dto.Bimestre,
            Descricao = dto.Descricao
        };

        var created = await _repository.CreateAsync(desenvolvimento);
        return Map(created);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var data = await _repository.GetByIdAsync(id);
        if (data == null) return false;

        await _repository.DeleteAsync(data);
        return true;
    }

    private static DesenvolvimentoMaternalResponseDto Map(DesenvolvimentoMaternal d) => new(
        d.Id,
        d.AlunoId,
        d.Aluno?.Nome ?? "",
        d.TurmaId,
        d.Turma?.Nome ?? "",
        d.AnoLetivoId,
        d.Bimestre,
        d.Descricao
    );
}