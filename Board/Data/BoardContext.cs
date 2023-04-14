using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Board.Models;
using Board.Infrastructure;

namespace Board.Data
{
    public class BoardContext : DbContext
    {
        public BoardContext (DbContextOptions<BoardContext> options)
            : base(options)
        {
        }

        public static DbContextOptions<BoardContext> Setting()
        {
            return new DbContextOptionsBuilder<BoardContext>()
                .UseSqlServer("Data Source =.\\SQLEXPRESS; Initial Catalog = board; Persist Security Info = True; User ID = sa; Password = 123qwe!@#QWE;Trusted_Connection=True;MultipleActiveResultSets=true;Trust Server Certificate=true")
                .Options;
        }

        public DbSet<Board.Models.Notice> Notice { get; set; } 
        public DbSet<Board.Models.Comments> Comments { get; set; }
        public DbSet<Board.Models.User> User { get; set; }
        public DbSet<Board.Models.FileModel> FileModel { get; set; }
        public DbSet<Board.Models.NoticeCategory> NoticeCategories { get; set; }

        //public DbSet<Board.Controllers.NoticeDto> noticeDto { get; set; }

    }
}
