using Board.Models;

namespace Board.Interfaces
{
    public interface LoginSessionRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<List<User>> ListAsync();
        Task AddAsync(User session);
        Task UpdateAsync(User session);
    }
}
