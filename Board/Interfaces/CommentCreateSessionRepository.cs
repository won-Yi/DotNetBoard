using Board.Models;

namespace Board.Interfaces
{
    public interface CommentCreateSessionRepository
    {
        Task<Comments> GetByIdAsync(int id);
        Task<List<Comments>> ListAsync();
        Task AddAsync(Comments session);
        Task UpdateAsync(Comments session);
    }
}
