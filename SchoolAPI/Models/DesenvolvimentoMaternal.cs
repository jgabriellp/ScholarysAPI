namespace SchoolAPI.Models;

public class DesenvolvimentoMaternal
{
    public int Id { get; set; }
    public int Bimestre { get; set; } // 1 a 4
    public string Descricao { get; set; } = string.Empty;

    // FKs
    public int AlunoId { get; set; }
    public Aluno Aluno { get; set; } = null!;

    public int TurmaId { get; set; }
    public Turma Turma { get; set; } = null!;

    public int AnoLetivoId { get; set; }
    public AnoLetivo AnoLetivo { get; set; } = null!;
}