namespace SchoolAPI.DTOs.Dashboard;

public record DashboardResponseDto(
    int TurmasCount,
    int AlunosCount,
    int DisciplinasCount,
    int ProfessoresCount
);