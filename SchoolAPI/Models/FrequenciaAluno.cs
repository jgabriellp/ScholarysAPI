namespace SchoolAPI.Models;

public class FrequenciaAluno
{
    public int Id { get; set; }
    public bool Presente { get; set; }

    // FKs
    public int FrequenciaId { get; set; }
    public Frequencia Frequencia { get; set; } = null!;

    public int AlunoId { get; set; }
    public Aluno Aluno { get; set; } = null!;
}