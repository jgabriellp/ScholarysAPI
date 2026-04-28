namespace SchoolAPI.Models;

public class Frequencia
{
    public int Id { get; set; }
    public DateOnly Data { get; set; }

    // FKs
    public int TurmaId { get; set; }
    public Turma Turma { get; set; } = null!;

    public int AnoLetivoId { get; set; }
    public AnoLetivo AnoLetivo { get; set; } = null!;

    // Navegação
    public ICollection<FrequenciaAluno> FrequenciaAlunos { get; set; } = [];
}