using SchoolAPI.Models;
using SchoolAPI.Models.Enum;

namespace SchoolAPI.Repositories.Interfaces;

public interface IDiaLetivoRepository
{
    Task<IEnumerable<DiaLetivo>> GetByAnoLetivoAsync(int anoLetivoId, SegmentoEnum? segmento = null);
    Task<DiaLetivo?> GetByIdAsync(int id);
    Task<IEnumerable<DiaLetivo>> CreateLoteAsync(IEnumerable<DiaLetivo> dias);
    Task DeleteAsync(DiaLetivo dia);
}
