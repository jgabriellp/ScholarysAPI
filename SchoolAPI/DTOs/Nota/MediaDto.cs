namespace SchoolAPI.DTOs.Nota;

public record MediaDto(
    int AlunoId,
    int DisciplinaId,
    decimal? Unidade1,
    decimal? Unidade2,
    decimal? Unidade3,
    decimal? Media1Semestre,
    decimal? Unidade4,
    decimal? Unidade5,
    decimal? Unidade6,
    decimal? Media2Semestre,
    decimal? MediaAnual,
    decimal? NotaRecuperacao,
    decimal? MediaFinal,
    string? Resultado
);