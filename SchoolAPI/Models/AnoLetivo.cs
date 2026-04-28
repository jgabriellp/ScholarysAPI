namespace SchoolAPI.Models;

public class AnoLetivo
{
    public int Id { get; set; }
    public int Ano { get; set; }
    public bool Ativo { get; set; } = true;

    // Navegação
    public ICollection<Turma> Turmas { get; set; } = [];
    public ICollection<Aluno> Alunos { get; set; } = [];
}