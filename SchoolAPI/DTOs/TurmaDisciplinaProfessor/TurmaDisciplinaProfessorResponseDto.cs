namespace SchoolAPI.DTOs.TurmaDisciplinaProfessor;

public record TurmaDisciplinaProfessorResponseDto(
    int Id,
    int TurmaId,
    string TurmaNome,
    int DisciplinaId,
    string DisciplinaNome,
    int ProfessorId,
    string ProfessorNome,
    int AnoLetivoId,
    int AnoLetivo
);