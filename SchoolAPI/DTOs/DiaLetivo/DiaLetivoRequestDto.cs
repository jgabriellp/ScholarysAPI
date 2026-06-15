namespace SchoolAPI.DTOs.DiaLetivo;

public record DiaLetivoLoteRequestDto(int AnoLetivoId, List<DateOnly> Datas);
