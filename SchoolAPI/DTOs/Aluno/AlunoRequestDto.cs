namespace SchoolAPI.DTOs.Aluno;

public record AlunoRequestDto(
    string Nome,
    int NumeroChamada,
    DateOnly DataNascimento,
    int TurmaId,
    int AnoLetivoId,
    int UserId
);