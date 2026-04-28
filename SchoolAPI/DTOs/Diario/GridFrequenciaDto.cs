namespace SchoolAPI.DTOs.Diario;

public record GridFrequenciaDto(
    IEnumerable<FrequenciaMesDto> Meses,
    int TotalFaltas,
    decimal Frequencia
);

public record FrequenciaMesDto(
    int Mes,
    string MesNome,
    IDictionary<int, bool?> Dias, // dia -> presente/ausente/null (sem aula)
    int TotalFaltas
);