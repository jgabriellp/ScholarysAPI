using SchoolAPI.Models;

namespace SchoolAPI.Repositories.Interfaces;

public interface IAnoLetivoRepository
{
    Task<(IEnumerable<AnoLetivo> Data, int Total)> GetAllAsync(int page, int pageSize);
    Task<AnoLetivo?> GetByIdAsync(int id);
    Task<AnoLetivo?> GetAtivoAsync();
    Task<AnoLetivo> CreateAsync(AnoLetivo anoLetivo);
    Task<AnoLetivo> UpdateAsync(AnoLetivo anoLetivo);
    Task DeleteAsync(AnoLetivo anoLetivo);
}