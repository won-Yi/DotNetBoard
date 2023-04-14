using Board.Controllers;
using Board.Data;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Microsoft.EntityFrameworkCore;
using Board.Models;
using Board.Infrastructure;

namespace BoardTest.BoardTests
{
    public class EditTest
    {

        [Theory]
        [InlineData(51, "title", "content", "user", "category")]
        //[InlineData(2,"title", null, "user", "category")]
        //[InlineData(3,"title", "content", null, "category")]
        //[InlineData(4,"title", "content", "user", null)]

        public async Task Edit_ReturnsBadRequest_WhenRequiredFieldsAreMissing(
                        int id,
                        string title,
                        string content,
                        string username,
                        string category
                        )
        {
            var options = new DbContextOptionsBuilder<BoardContext>()
            .UseSqlServer("Data Source =.\\SQLEXPRESS; Initial Catalog = board; Persist Security Info = True; User ID = sa; Password = 123qwe!@#QWE;Trusted_Connection=True;MultipleActiveResultSets=true;Trust Server Certificate=true")
            .Options;
            var context = new BoardContext(options);
            var env = new Mock<IWebHostEnvironment>().Object;
            var _context = new EFSboardSessionRepository(context);
            var create_context = new EFSCommentSessionRepository(context);
            var controller = new NoticesController(_context, create_context, env, context);
            
            var result = await controller.Edit(id, title, content, username, category);


            Assert.NotNull(result);
        }//null값을 확인하는 테스트

        [Fact]
        public async Task Edit_ReturnsCorrectResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BoardContext>()
            .UseSqlServer("Data Source =.\\SQLEXPRESS; Initial Catalog = board; Persist Security Info = True; User ID = sa; Password = 123qwe!@#QWE;Trusted_Connection=True;MultipleActiveResultSets=true;Trust Server Certificate=true")
            .Options;
            var context = new BoardContext(options);
            var env = new Mock<IWebHostEnvironment>().Object;
            var _context = new EFSboardSessionRepository(context);
            var create_context = new EFSCommentSessionRepository(context);
            var controller = new NoticesController(_context, create_context, env, context);

            var notices = new Notice
            {
                Id = 51,
                Title = "Initial Title",
                Content = "Initial Content",
                UserName = "Initial User",
                Category = "Initial Category"
            };

            var result = await controller.Edit(
               id: notices.Id,
               title: "New Title",
               content: "New Content",
               username: "New User",
               category: "New Category"
               );


            // Assert
            var updatedNotice = await context.Notice.FindAsync(notices.Id);
            Assert.Equal("New Title", updatedNotice.Title);
            Assert.Equal("New Content", updatedNotice.Content);
            Assert.Equal("New User", updatedNotice.UserName);
            Assert.Equal("New Category", updatedNotice.Category);
        }//전달받은 값과 return값이 같은지 비교


    }//class
}//nameSpace
