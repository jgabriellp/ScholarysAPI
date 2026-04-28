namespace SchoolAPI.Models;

public class Aluno
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int NumeroChamada { get; set; }
    public DateOnly DataNascimento { get; set; }
    public bool Ativo { get; set; } = true;

    // FK
    public int TurmaId { get; set; }
    public Turma Turma { get; set; } = null!;

    public int AnoLetivoId { get; set; }
    public AnoLetivo AnoLetivo { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    // Navegação
    public ICollection<FrequenciaAluno> Frequencias { get; set; } = [];
    public ICollection<Nota> Notas { get; set; } = [];
    public ICollection<DesenvolvimentoMaternal> Desenvolvimentos { get; set; } = [];
}