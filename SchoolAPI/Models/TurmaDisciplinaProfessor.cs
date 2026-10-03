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

    // Dias da semana com aula (valores de DayOfWeek: 1 = segunda ... 5 = sexta).
    // Vazio = aula em todos os dias letivos.
    public List<int> DiasSemana { get; set; } = [];
}