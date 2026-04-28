namespace SchoolAPI.DTOs.Nota;

public record NotaRequestDto(
    int AlunoId,
    int DisciplinaId,
    int TurmaId,
    int AnoLetivoId,
    int Unidade,
    decimal Valor
);