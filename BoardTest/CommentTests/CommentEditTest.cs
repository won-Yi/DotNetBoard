using Board.Controllers;
using Board.Data;
using Board.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardTest.CommentTests
{
    public class CommentEditTest
    {
        [Theory]
        [InlineData(2,"EditUser", "EditComment")]
        //test function만들기
        public async Task CommentEdit_ReturnBadRequest_WhenRequiredFiedlsAreMissing(
                    int id,
                    string UserName,
                    string editComment

            ){
            //settings 
            var options = BoardContext.Setting();
            var context = new BoardContext(options);
            var env = new Mock<IWebHostEnvironment>().Object;
            var _context = new EFSboardSessionRepository(context); // 구현한 클래스의 인스턴스 생성
            var create_context = new EFSCommentSessionRepository(context);
            var controller = new NoticesController(_context, create_context, env, context);

            //값 전달

            var result = await controller.CommentEdit(id, UserName, editComment);

            //assert

            Assert.NotNull(result);
        }

        [Fact]
        public async Task CommentEdit_ReturnCorrectResult() {
            
            int id = 2;

            //settings 
            var options = BoardContext.Setting();
            var context = new BoardContext(options);
            var env = new Mock<IWebHostEnvironment>().Object;
            var _context = new EFSboardSessionRepository(context); // 구현한 클래스의 인스턴스 생성
            var create_context = new EFSCommentSessionRepository(context);
            var controller = new NoticesController(_context, create_context, env, context);


            //원래 초기값을 꼭 넣어줘야 하나?

            //새로운 값을 넣어준다. 
            var result = await controller.CommentEdit(
                    Id: id,
                    UserName: "New Name",
                    editComment : "New Comment"
                );

            var comment = await context.Comments.FindAsync(id);
            //assert
            Assert.NotNull(result);
            Assert.Equal(id, comment.Id);
            Assert.Equal("New Name", comment.UserName);
            Assert.Equal("New Comment", comment.Comment);
        }

    }//class
}//namespace
