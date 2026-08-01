using SchoolAPI.DTOs.RelatoAula;
using SchoolAPI.Models;
using SchoolAPI.Repositories.Interfaces;

namespace SchoolAPI.Services;

public class RelatoAulaService
{
    private readonly IRelatoAulaRepository _repository;
    private readonly IDiaLetivoRepository _diaLetivoRepository;
    private readonly ITurmaRepository _turmaRepository;

    public RelatoAulaService(
        IRelatoAulaRepository repository,
        IDiaLetivoRepository diaLetivoRepository,
        ITurmaRepository turmaRepository)
    {
        _repository = repository;
        _diaLetivoRepository = diaLetivoRepository;
        _turmaRepository = turmaRepository;
    }

    public async Task<IEnumerable<RelatoAulaResponseDto>> GetByTurmaEAnoAsync(int turmaId, int anoLetivoId)
    {
        var relatos = await _repository.GetByTurmaEAnoAsync(turmaId, anoLetivoId);
        return relatos.Select(Map);
    }

    public async Task<RelatoAulaResponseDto> UpsertAsync(RelatoAulaRequestDto dto)
    {
        var diaLetivo = await _diaLetivoRepository.GetByIdAsync(dto.DiaLetivoId)
            ?? throw new ArgumentException("Dia letivo não encontrado.");

        var turma = await _turmaRepository.GetByIdAsync(dto.TurmaId)
            ?? throw new ArgumentException("Turma não encontrada.");

        if (turma.Segmento != diaLetivo.Segmento)
            throw new ArgumentException(
                $"O dia letivo selecionado pertence ao segmento {diaLetivo.Segmento}, incompatível com o segmento {turma.Segmento} da turma.");

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
