using SchoolAPI.Models.Enum;

namespace SchoolAPI.DTOs.DiaLetivo;

public record DiaLetivoLoteRequestDto(int AnoLetivoId, SegmentoEnum Segmento, List<DateOnly> Datas);
