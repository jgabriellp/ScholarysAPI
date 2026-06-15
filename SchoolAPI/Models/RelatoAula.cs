namespace SchoolAPI.Models;

public class RelatoAula
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;

    public int DiaLetivoId { get; set; }
    public DiaLetivo DiaLetivo { get; set; } = null!;

    public int TurmaId { get; set; }
    public Turma Turma { get; set; } = null!;

    public int ProfessorId { get; set; }
    public User Professor { get; set; } = null!;
}
