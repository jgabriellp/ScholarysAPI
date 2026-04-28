using SchoolAPI.Models.Enum;

namespace SchoolAPI.Models;

public class User
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public RoleEnum Role { get; set; }
    public bool Ativo { get; set; } = true;

    // Navegação
    public Aluno? Aluno { get; set; }
}