using SchoolAPI.DTOs.DiaLetivo;
using SchoolAPI.Models;
using SchoolAPI.Repositories.Interfaces;

namespace SchoolAPI.Services;

public class DiaLetivoService
{
    private readonly IDiaLetivoRepository _repository;

    public DiaLetivoService(IDiaLetivoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DiaLetivoResponseDto>> GetByAnoLetivoAsync(int anoLetivoId)
    {
        var dias = await _repository.GetByAnoLetivoAsync(anoLetivoId);
        return dias.Select(Map);
    }

    public async Task<IEnumerable<DiaLetivoResponseDto>> CreateLoteAsync(DiaLetivoLoteRequestDto dto)
    {
        var dias = dto.Datas.Select(data => new DiaLetivo
        {
            AnoLetivoId = dto.AnoLetivoId,
            Data = data
        });

        var criados = await _repository.CreateLoteAsync(dias);
        return criados.Select(Map);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var dia = await _repository.GetByIdAsync(id);
        if (dia == null) return false;

        await _repository.DeleteAsync(dia);
        return true;
    }

    private static DiaLetivoResponseDto Map(DiaLetivo d) => new(
        d.Id,
        d.AnoLetivoId,
        d.AnoLetivo?.Ano ?? 0,
        d.Data
    );
}
