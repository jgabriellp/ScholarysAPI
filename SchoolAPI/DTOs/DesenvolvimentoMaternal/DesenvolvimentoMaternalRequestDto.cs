namespace SchoolAPI.DTOs.DesenvolvimentoMaternal;

public record DesenvolvimentoMaternalRequestDto(
    int AlunoId,
    int TurmaId,
    int AnoLetivoId,
    int Bimestre,
    string Descricao
);