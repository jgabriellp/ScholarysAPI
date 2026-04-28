namespace SchoolAPI.DTOs.Aluno;

public record AlunoResponseDto(
    int Id,
    string Nome,
    int NumeroChamada,
    DateOnly DataNascimento,
    int TurmaId,
    string TurmaNome,
    int AnoLetivoId,
    int AnoLetivo,
    int UserId,
    bool Ativo
);