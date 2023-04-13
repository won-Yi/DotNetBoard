using Board.Controllers;
using Board.Data;
using Board.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Diagnostics.Runtime;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using Board.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Board.Models;

namespace BoardTest.BoardTests
{
    public class IndexTest
    {
        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfBrainstormSessions()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BoardContext>()
             .UseSqlServer("Data Source =.\\SQLEXPRESS; Initial Catalog = board; Persist Security Info = True; User ID = sa; Password = 123qwe!@#QWE;Trusted_Connection=True;MultipleActiveResultSets=true;Trust Server Certificate=true")
             .Options;
            var context = new BoardContext(options);
            var env = new Mock<IWebHostEnvironment>().Object;
            var _context = new EFSboardSessionRepository(context);
            var controller = new NoticesController(_context, env, context);

            // Act

            var result = await controller.Index(null, null, 2);

            // Assert
            //var viewResult = Assert.IsType<ViewResult>(result);
            //var Model = Assert.IsAssignableFrom<IEnumerable<NoticeDto>>(viewResult.ViewData.Model);
            // 모델 가져오기
            //var model = Assert.IsType<NoticeDto>(viewResult.ViewData.Model);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = from m in context.Notice select m;


            foreach (var noticeItem in model)
            {
                Assert.NotEmpty(noticeItem.Title);
                Assert.NotEmpty(noticeItem.Content);
                Assert.NotEmpty(noticeItem.Category);
            }
            var expectedCount = context.Notice.Count();
            Assert.Equal(expectedCount, model.Count());
        }
    }
}
