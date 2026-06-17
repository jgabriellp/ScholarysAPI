using SchoolAPI.Models.Enum;

namespace SchoolAPI.DTOs.Disciplina;

public record DisciplinaRequestDto(string Nome, SegmentoEnum Segmento);
