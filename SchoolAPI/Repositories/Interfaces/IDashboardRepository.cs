using SchoolAPI.DTOs.Dashboard;

namespace SchoolAPI.Repositories.Interfaces;

public interface IDashboardRepository
{
    Task<DashboardResponseDto> GetDashboardInfoAsync();
}