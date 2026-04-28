namespace SchoolAPI.DTOs.Diario;

public record DiarioFundamentalDto(
    int AlunoId,
    string AlunoNome,
    int NumeroChamada,
    string TurmaNome,
    int AnoLetivo,
    GridFrequenciaDto Frequencia,
    IEnumerable<NotaDisciplinaDto> Notas,
    string? Observacoes,
    string? Resultado
);

public record NotaDisciplinaDto(
    int DisciplinaId,
    string DisciplinaNome,
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