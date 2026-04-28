namespace SchoolAPI.Models;

public class TurmaDisciplinaProfessor
{
    public int Id { get; set; }

    // FKs
    public int TurmaId { get; set; }
    public Turma Turma { get; set; } = null!;

    public int DisciplinaId { get; set; }
    public Disciplina Disciplina { get; set; } = null!;

    public int ProfessorId { get; set; }
    public User Professor { get; set; } = null!;

    public int AnoLetivoId { get; set; }
    public AnoLetivo AnoLetivo { get; set; } = null!;
}