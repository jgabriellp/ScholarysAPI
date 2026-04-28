using SchoolAPI.DTOs.Nota;
using SchoolAPI.Models;
using SchoolAPI.Repositories.Interfaces;

namespace SchoolAPI.Services;

public class NotaService
{
    private readonly INotaRepository _repository;

    public NotaService(INotaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<NotaResponseDto>> GetByAlunoAsync(int alunoId, int anoLetivoId)
    {
        var notas = await _repository.GetByAlunoAsync(alunoId, anoLetivoId);
        return notas.Select(Map);
    }

    public async Task<IEnumerable<NotaResponseDto>> GetByTurmaEDisciplinaAsync(int turmaId, int disciplinaId, int anoLetivoId)
    {
        var notas = await _repository.GetByTurmaEDisciplinaAsync(turmaId, disciplinaId, anoLetivoId);
        return notas.Select(Map);
    }

    public async Task<NotaResponseDto> LancarAsync(NotaRequestDto dto)
    {
        if (dto.Valor < 0 || dto.Valor > 10)
            throw new ArgumentException("Nota deve ser entre 0 e 10.");

        if (dto.Unidade < 1 || dto.Unidade > 7)
            throw new ArgumentException("Unidade deve ser entre 1 e 7.");

        // Unidade 7 = Recuperação Final, só pode ser lançada se média anual < 6
        var existente = await _repository.GetByAlunoUnidadeAsync(
            dto.AlunoId, dto.DisciplinaId, dto.Unidade, dto.AnoLetivoId);

        if (existente != null)
        {
            existente.Valor = dto.Valor;
            var updated = await _repository.UpdateAsync(existente);
            return Map(updated);
        }

        var nota = new Nota
        {
            AlunoId = dto.AlunoId,
            DisciplinaId = dto.DisciplinaId,
            TurmaId = dto.TurmaId,
            AnoLetivoId = dto.AnoLetivoId,
            Unidade = dto.Unidade,
            Valor = dto.Valor
        };

        var created = await _repository.CreateAsync(nota);
        return Map(created);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var nota = await _repository.GetByIdAsync(id);
        if (nota == null) return false;

        await _repository.DeleteAsync(nota);
        return true;
    }

    // Calcula médias de um aluno em uma disciplina
    public async Task<MediaDto> CalcularMediaAsync(int alunoId, int disciplinaId, int anoLetivoId)
    {
        var notas = (await _repository.GetByAlunoAsync(alunoId, anoLetivoId))
            .Where(n => n.DisciplinaId == disciplinaId)
            .ToList();

        decimal? GetNota(int unidade) => notas
            .FirstOrDefault(n => n.Unidade == unidade)?.Valor;

        var u1 = GetNota(1);
        var u2 = GetNota(2);
        var u3 = GetNota(3);
        var u4 = GetNota(4);
        var u5 = GetNota(5);
        var u6 = GetNota(6);
        var rec = GetNota(7);

        decimal? media1Sem = (u1.HasValue && u2.HasValue && u3.HasValue)
            ? Math.Round((u1.Value + u2.Value + u3.Value) / 3, 2) : null;

        decimal? media2Sem = (u4.HasValue && u5.HasValue && u6.HasValue)
            ? Math.Round((u4.Value + u5.Value + u6.Value) / 3, 2) : null;

        decimal? mediaAnual = (media1Sem.HasValue && media2Sem.HasValue)
            ? Math.Round((media1Sem.Value + media2Sem.Value) / 2, 2) : null;

        decimal? mediaFinal = null;
        string? resultado = null;

        if (mediaAnual.HasValue)
        {
            if (mediaAnual.Value >= 6)
            {
                mediaFinal = mediaAnual;
                resultado = "Aprovado";
            }
            else if (rec.HasValue)
            {
                mediaFinal = Math.Round((mediaAnual.Value + rec.Value) / 2, 2);
                resultado = mediaFinal >= 6 ? "Aprovado" : "Reprovado";
            }
        }

        return new MediaDto(
            alunoId, disciplinaId,
            u1, u2, u3, media1Sem,
            u4, u5, u6, media2Sem,
            mediaAnual, rec, mediaFinal, resultado
        );
    }

    private static NotaResponseDto Map(Nota n) => new(
        n.Id,
        n.AlunoId,
        n.Aluno?.Nome ?? "",
        n.DisciplinaId,
        n.Disciplina?.Nome ?? "",
        n.TurmaId,
        n.Unidade,
        n.Valor
    );
}