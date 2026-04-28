using SchoolAPI.Models.Enum;

namespace SchoolAPI.DTOs.Turma;

public record TurmaRequestDto(
    string Nome,
    SegmentoEnum Segmento,
    int AnoLetivoId
);