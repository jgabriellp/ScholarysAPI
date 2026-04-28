namespace SchoolAPI.DTOs.Diario;

public record DiarioMaternalDto(
    int AlunoId,
    string AlunoNome,
    int NumeroChamada,
    string TurmaNome,
    int AnoLetivo,
    GridFrequenciaDto Frequencia,
    IEnumerable<DesenvolvimentoBimestralDto> Desenvolvimentos
);

public record DesenvolvimentoBimestralDto(
    int Bimestre,
    string Descricao
);