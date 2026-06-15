namespace SchoolAPI.DTOs.RelatoAula;

public record RelatoAulaResponseDto(
    int Id,
    int DiaLetivoId,
    DateOnly Data,
    int TurmaId,
    string TurmaNome,
    int ProfessorId,
    string ProfessorNome,
    string Descricao
);
