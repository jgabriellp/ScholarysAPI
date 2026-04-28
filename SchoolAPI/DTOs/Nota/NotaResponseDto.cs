namespace SchoolAPI.DTOs.Nota;

public record NotaResponseDto(
    int Id,
    int AlunoId,
    string AlunoNome,
    int DisciplinaId,
    string DisciplinaNome,
    int TurmaId,
    int Unidade,
    decimal Valor
);