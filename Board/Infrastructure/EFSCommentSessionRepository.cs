using Board.Data;
using Board.Interfaces;
using Board.Models;
using Microsoft.EntityFrameworkCore;

namespace Board.Infrastructure
{
    public class EFSCommentSessionRepository: CommentCreateSessionRepository
    {

        private readonly BoardContext _context;

        public EFSCommentSessionRepository(BoardContext dbContext) {
            
            _context  = dbContext;
        }

        //Comment
        public Task<Comments> GetByIdAsync(int id)
        {
            return _context.Comments
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public Task<List<Comments>> ListAsync()
        {
            return _context.Comments
                .OrderByDescending(s => s.UpdateTime)
                .ToListAsync();
        }

        public Task AddAsync(Comments session)
        {
            _context.Comments.Add(session);
            return _context.SaveChangesAsync();
        }

        public Task UpdateAsync(Comments session)
        {
            
            _context.Entry(session).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }
    }
}
