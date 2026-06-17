using SchoolAPI.Models.Enum;

namespace SchoolAPI.DTOs.Disciplina;

public record DisciplinaResponseDto(int Id, string Nome, SegmentoEnum Segmento, bool Ativo);
