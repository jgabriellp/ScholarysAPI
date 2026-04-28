using SchoolAPI.Models;

namespace SchoolAPI.Repositories.Interfaces;

public interface IUserRepository
{
    Task<(IEnumerable<User> Data, int Total)> GetAllAsync(int page, int pageSize);
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
    Task DeleteAsync(User user);
}