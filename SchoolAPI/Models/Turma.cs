using SchoolAPI.Models.Enum;

namespace SchoolAPI.Models;

public class Turma
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public SegmentoEnum Segmento { get; set; }
    public bool Ativo { get; set; } = true;

    // FK
    public int AnoLetivoId { get; set; }
    public AnoLetivo AnoLetivo { get; set; } = null!;

    // Navegação
    public ICollection<Aluno> Alunos { get; set; } = [];
    public ICollection<TurmaDisciplinaProfessor> TurmaDisciplinaProfessores { get; set; } = [];
    public ICollection<Frequencia> Frequencias { get; set; } = [];
}