namespace SchoolAPI.DTOs.TurmaDisciplinaProfessor;

public record TurmaDisciplinaProfessorRequestDto(
    int TurmaId,
    int DisciplinaId,
    int ProfessorId,
    int AnoLetivoId
);