using SchoolAPI.DTOs.AnoLetivo;
using SchoolAPI.Models;
using SchoolAPI.Repositories.Interfaces;

namespace SchoolAPI.Services;

public class AnoLetivoService
{
    private readonly IAnoLetivoRepository _repository;

    public AnoLetivoService(IAnoLetivoRepository repository)
    {
        _repository = repository;
    }

    public async Task<(IEnumerable<AnoLetivoResponseDto> Data, int Total)> GetAllAsync(int page, int pageSize)
    {
        var (anos, total) = await _repository.GetAllAsync(page, pageSize);
        var data = anos.Select(a => new AnoLetivoResponseDto(a.Id, a.Ano, a.Ativo));
        return (data, total);
    }

    public async Task<AnoLetivoResponseDto?> GetByIdAsync(int id)
    {
        var ano = await _repository.GetByIdAsync(id);
        if (ano == null) return null;
        return new AnoLetivoResponseDto(ano.Id, ano.Ano, ano.Ativo);
    }

    public async Task<AnoLetivoResponseDto?> GetAtivoAsync()
    {
        var ano = await _repository.GetAtivoAsync();
        if (ano == null) return null;
        return new AnoLetivoResponseDto(ano.Id, ano.Ano, ano.Ativo);
    }

    public async Task<AnoLetivoResponseDto> CreateAsync(AnoLetivoRequestDto dto)
    {
        var anoLetivo = new AnoLetivo
        {
            Ano = dto.Ano,
            Ativo = true
        };

        var created = await _repository.CreateAsync(anoLetivo);
        return new AnoLetivoResponseDto(created.Id, created.Ano, created.Ativo);
    }

    public async Task<AnoLetivoResponseDto?> UpdateAsync(int id, AnoLetivoRequestDto dto)
    {
        var ano = await _repository.GetByIdAsync(id);
        if (ano == null) return null;

        ano.Ano = dto.Ano;

        var updated = await _repository.UpdateAsync(ano);
        return new AnoLetivoResponseDto(updated.Id, updated.Ano, updated.Ativo);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var ano = await _repository.GetByIdAsync(id);
        if (ano == null) return false;

        await _repository.DeleteAsync(ano);
        return true;
    }
}