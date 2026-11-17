using SchoolAPI.DTOs.Dashboard;
using SchoolAPI.Repositories.Interfaces;

namespace SchoolAPI.Services;

public class DashboardService
{
    private readonly IDashboardRepository _repository;
    private readonly IAnoLetivoRepository _anoLetivoRepository;

    public DashboardService(IDashboardRepository repository, IAnoLetivoRepository anoLetivoRepository)
    {
        _repository = repository;
        _anoLetivoRepository = anoLetivoRepository;
    }

    // Sem anoLetivoId, usa o ano letivo corrente.
    public async Task<DashboardResponseDto> GetDashboardInfoAsync(int? anoLetivoId = null)
    {
        anoLetivoId ??= (await _anoLetivoRepository.GetAtivoAsync())?.Id;
        return await _repository.GetDashboardInfoAsync(anoLetivoId);
    }
}
