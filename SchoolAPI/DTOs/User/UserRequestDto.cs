using SchoolAPI.Models.Enum;

namespace SchoolAPI.DTOs.User;

public record UserRequestDto(
    string Nome,
    string Email,
    string Password,
    RoleEnum Role
);