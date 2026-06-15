namespace SchoolAPI.DTOs.RelatoAula;

public record RelatoAulaRequestDto(int DiaLetivoId, int TurmaId, int ProfessorId, string Descricao);
