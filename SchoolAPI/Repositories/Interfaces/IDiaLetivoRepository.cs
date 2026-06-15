using SchoolAPI.Models;

namespace SchoolAPI.Repositories.Interfaces;

public interface IDiaLetivoRepository
{
    Task<IEnumerable<DiaLetivo>> GetByAnoLetivoAsync(int anoLetivoId);
    Task<DiaLetivo?> GetByIdAsync(int id);
    Task<IEnumerable<DiaLetivo>> CreateLoteAsync(IEnumerable<DiaLetivo> dias);
    Task DeleteAsync(DiaLetivo dia);
}
