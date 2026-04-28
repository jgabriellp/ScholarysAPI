namespace SchoolAPI.Models;

public class Nota
{
    public int Id { get; set; }
    public int Unidade { get; set; } // 1 a 6, 7 = Recuperação Final
    public decimal Valor { get; set; }

    // FKs
    public int AlunoId { get; set; }
    public Aluno Aluno { get; set; } = null!;

    public int DisciplinaId { get; set; }
    public Disciplina Disciplina { get; set; } = null!;

    public int TurmaId { get; set; }
    public Turma Turma { get; set; } = null!;

    public int AnoLetivoId { get; set; }
    public AnoLetivo AnoLetivo { get; set; } = null!;
}