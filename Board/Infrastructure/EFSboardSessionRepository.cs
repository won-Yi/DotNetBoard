
using Board.Data;
using Board.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Board.Interfaces;
using System.Diagnostics;

namespace Board.Infrastructure
{  
    public class EFSboardSessionRepository:CreateSessionRepository
        //EditSessionRepository
    {

        private readonly BoardContext _context;
        public EFSboardSessionRepository(BoardContext dbContext)
        {
            _context = dbContext;
        }
        
        //Notice
        public async Task<Notice> GetByIdAsync(int id)
        {
            return await _context.Notice.FindAsync(id);
        }

        public Task<List<Notice>> ListAsync()
        {
            return _context.Notice
                .OrderByDescending(s => s.UpdateDate)
                .ToListAsync();
        }

        public Task AddAsync(Notice session)
        {
            _context.Notice.Add(session);
            return _context.SaveChangesAsync();
        }

        public Task UpdateAsync(Notice session)
        {
            //var entry = _context.ChangeTracker.Entries<Notice>()
            //.FirstOrDefault(e => e.Entity.Id == session.Id);

            //if (entry != null)
            //{
            //    entry.State = EntityState.Detached; // 중복된 엔티티 분리
            //}

            //_context.Notice.Attach(session);
            _context.Entry(session).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

    }//class
}//namespace
