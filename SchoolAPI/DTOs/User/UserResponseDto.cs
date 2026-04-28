using SchoolAPI.Models.Enum;

namespace SchoolAPI.DTOs.User;

public record UserResponseDto(
    int Id,
    string Nome,
    string Email,
    RoleEnum Role,
    bool Ativo
);