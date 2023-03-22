using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Board.Models;

namespace Board.Data
{
    public class BoardContext : DbContext
    {
        public BoardContext (DbContextOptions<BoardContext> options)
            : base(options)
        {
        }

        public DbSet<Board.Models.Notice> Notice { get; set; } 
        public DbSet<Board.Models.Comments> Comments { get; set; }
    


    }
}
