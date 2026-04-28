using SchoolAPI.DTOs.User;
using SchoolAPI.Models;
using SchoolAPI.Repositories.Interfaces;

namespace SchoolAPI.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<(IEnumerable<UserResponseDto> Data, int Total)> GetAllAsync(int page, int pageSize)
    {
        var (users, total) = await _userRepository.GetAllAsync(page, pageSize);
        var data = users.Select(u => new UserResponseDto(u.Id, u.Nome, u.Email, u.Role, u.Ativo));
        return (data, total);
    }

    public async Task<UserResponseDto?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return null;
        return new UserResponseDto(user.Id, user.Nome, user.Email, user.Role, user.Ativo);
    }

    public async Task<UserResponseDto> CreateAsync(UserRequestDto dto)
    {
        var user = new User
        {
            Nome = dto.Nome,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role,
            Ativo = true
        };

        var created = await _userRepository.CreateAsync(user);
        return new UserResponseDto(created.Id, created.Nome, created.Email, created.Role, created.Ativo);
    }

    public async Task<UserResponseDto?> UpdateAsync(int id, UserRequestDto dto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return null;

        user.Nome = dto.Nome;
        user.Email = dto.Email;
        user.Role = dto.Role;

        if (!string.IsNullOrEmpty(dto.Password))
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var updated = await _userRepository.UpdateAsync(user);
        return new UserResponseDto(updated.Id, updated.Nome, updated.Email, updated.Role, updated.Ativo);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return false;

        await _userRepository.DeleteAsync(user);
        return true;
    }
}