using SchoolAPI.DTOs.RelatoAula;
using SchoolAPI.Models;
using SchoolAPI.Repositories.Interfaces;

namespace SchoolAPI.Services;

public class RelatoAulaService
{
    private readonly IRelatoAulaRepository _repository;

    public RelatoAulaService(IRelatoAulaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<RelatoAulaResponseDto>> GetByTurmaEAnoAsync(int turmaId, int anoLetivoId)
    {
        var relatos = await _repository.GetByTurmaEAnoAsync(turmaId, anoLetivoId);
        return relatos.Select(Map);
    }

    public async Task<RelatoAulaResponseDto> UpsertAsync(RelatoAulaRequestDto dto)
    {
        var existente = await _repository.GetByDiaETurmaEProfessorAsync(dto.DiaLetivoId, dto.TurmaId, dto.ProfessorId);

        if (existente != null)
        {
            existente.Descricao = dto.Descricao;
            var atualizado = await _repository.UpdateAsync(existente);
            return Map(atualizado);
        }

        var novo = new RelatoAula
        {
            DiaLetivoId = dto.DiaLetivoId,
            TurmaId = dto.TurmaId,
            ProfessorId = dto.ProfessorId,
            Descricao = dto.Descricao
        };

        var criado = await _repository.CreateAsync(novo);
        return Map(criado);
    }

    private static RelatoAulaResponseDto Map(RelatoAula r) => new(
        r.Id,
        r.DiaLetivoId,
        r.DiaLetivo?.Data ?? default,
        r.TurmaId,
        r.Turma?.Nome ?? "",
        r.ProfessorId,
        r.Professor?.Nome ?? "",
        r.Descricao
    );
}
