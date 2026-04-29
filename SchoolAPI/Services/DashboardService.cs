using SchoolAPI.DTOs.Dashboard;
using SchoolAPI.Repositories.Interfaces;

namespace SchoolAPI.Services;

public class DashboardService
{
    private readonly IDashboardRepository _repository;

    public DashboardService(IDashboardRepository repository)
    {
        _repository = repository;
    }

    public Task<DashboardResponseDto> GetDashboardInfoAsync() =>
        _repository.GetDashboardInfoAsync();
}
