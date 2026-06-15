namespace SchoolAPI.Models;

public class DiaLetivo
{
    public int Id { get; set; }
    public DateOnly Data { get; set; }

    public int AnoLetivoId { get; set; }
    public AnoLetivo AnoLetivo { get; set; } = null!;

    public ICollection<RelatoAula> Relatos { get; set; } = [];
}
