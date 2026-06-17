using SchoolAPI.Models.Enum;

namespace SchoolAPI.Models;

public class Disciplina
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public SegmentoEnum Segmento { get; set; }
    public bool Ativo { get; set; } = true;

    // Navegação
    public ICollection<TurmaDisciplinaProfessor> TurmaDisciplinaProfessores { get; set; } = [];
    public ICollection<Nota> Notas { get; set; } = [];
}
