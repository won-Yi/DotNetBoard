using Board.Models;

namespace Board.Interfaces
{
    public interface EditSessionRepository
    {
        Task<Notice> GetByIdAsync(int id);
        Task<List<Notice>> ListAsync();
        Task AddAsync(Notice session);
        Task UpdateAsync(Notice session);
    }
}
