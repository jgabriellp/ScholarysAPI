using SchoolAPI.DTOs.Turma;
using SchoolAPI.Models;
using SchoolAPI.Repositories.Interfaces;

namespace SchoolAPI.Services;

public class TurmaService
{
    private readonly ITurmaRepository _repository;

    public TurmaService(ITurmaRepository repository)
    {
        _repository = repository;
    }

    public async Task<(IEnumerable<TurmaResponseDto> Data, int Total)> GetAllAsync(int page, int pageSize)
    {
        var (turmas, total) = await _repository.GetAllAsync(page, pageSize);
        var data = turmas.Select(Map);
        return (data, total);
    }

    public async Task<IEnumerable<TurmaResponseDto>> GetByAnoLetivoAsync(int anoLetivoId)
    {
        var turmas = await _repository.GetByAnoLetivoAsync(anoLetivoId);
        return turmas.Select(Map);
    }

    public async Task<TurmaResponseDto?> GetByIdAsync(int id)
    {
        var turma = await _repository.GetByIdAsync(id);
        if (turma == null) return null;
        return Map(turma);
    }

    public async Task<TurmaResponseDto> CreateAsync(TurmaRequestDto dto)
    {
        var turma = new Turma
        {
            Nome = dto.Nome,
            Segmento = dto.Segmento,
            AnoLetivoId = dto.AnoLetivoId,
            Ativo = true
        };

        var created = await _repository.CreateAsync(turma);
        return Map(created);
    }

    public async Task<TurmaResponseDto?> UpdateAsync(int id, TurmaRequestDto dto)
    {
        var turma = await _repository.GetByIdAsync(id);
        if (turma == null) return null;

        turma.Nome = dto.Nome;
        turma.Segmento = dto.Segmento;
        turma.AnoLetivoId = dto.AnoLetivoId;

        var updated = await _repository.UpdateAsync(turma);
        return Map(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var turma = await _repository.GetByIdAsync(id);
        if (turma == null) return false;

        await _repository.DeleteAsync(turma);
        return true;
    }

    private static TurmaResponseDto Map(Turma t) => new(
        t.Id,
        t.Nome,
        t.Segmento,
        t.AnoLetivoId,
        t.AnoLetivo?.Ano.ToString() ?? "",
        t.Ativo
    );
}
