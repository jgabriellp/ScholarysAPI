using SchoolAPI.Models.Enum;

namespace SchoolAPI.DTOs.Turma;

public record TurmaResponseDto(
    int Id,
    string Nome,
    SegmentoEnum Segmento,
    int AnoLetivoId,
    string AnoLetivoAno,
    bool Ativo
);