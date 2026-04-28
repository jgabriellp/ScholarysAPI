namespace SchoolAPI.DTOs.DesenvolvimentoMaternal;

public record DesenvolvimentoMaternalResponseDto(
    int Id,
    int AlunoId,
    string AlunoNome,
    int TurmaId,
    string TurmaNome,
    int AnoLetivoId,
    int Bimestre,
    string Descricao
);