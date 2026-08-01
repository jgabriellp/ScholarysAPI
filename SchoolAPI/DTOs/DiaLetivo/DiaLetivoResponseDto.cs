using SchoolAPI.Models.Enum;

namespace SchoolAPI.DTOs.DiaLetivo;

public record DiaLetivoResponseDto(int Id, int AnoLetivoId, int AnoLetivoAno, DateOnly Data, SegmentoEnum Segmento);
