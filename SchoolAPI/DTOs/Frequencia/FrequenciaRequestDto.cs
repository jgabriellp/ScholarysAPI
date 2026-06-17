namespace SchoolAPI.DTOs.Frequencia;

public record FrequenciaAlunoItemDto(int AlunoId, bool Presente, string? Observacao = null);

public record FrequenciaRequestDto(
    int TurmaId,
    int AnoLetivoId,
    DateOnly Data,
    List<FrequenciaAlunoItemDto> Alunos
);