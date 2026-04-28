namespace SchoolAPI.DTOs.Frequencia;

public record FrequenciaAlunoResponseDto(
    int AlunoId,
    string AlunoNome,
    bool Presente
);

public record FrequenciaResponseDto(
    int Id,
    int TurmaId,
    DateOnly Data,
    List<FrequenciaAlunoResponseDto> Alunos
);