using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftWindowsTCPIP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Board.Controllers;
using Board.Models;
using Microsoft.EntityFrameworkCore;
using Board.Data;
using Microsoft.AspNetCore.Hosting;
using Board.Infrastructure;
using Board.Controllers;
using static Dropbox.Api.Files.SearchMatchType;
using Microsoft.AspNetCore.Mvc;

namespace BoardTest.CommentTests
{

   
    public class CommentCreateTest
    {
        [Theory]
        [InlineData(53,"UserName", "Comment")]
        public async Task CommentCreate_Return_WhenRequiredFiledsAerMissing(
            int id, 
            string Username,
            string Comment
        ){
            var options = BoardContext.Setting();
            var context = new BoardContext(options);
            var env = new Mock<IWebHostEnvironment>().Object;
            var _context = new EFSboardSessionRepository(context); // 구현한 클래스의 인스턴스 생성
            var create_context = new EFSCommentSessionRepository(context);
            var controller = new NoticesController(_context, create_context, env, context);


            //controller에 id,username,comment를 전달해줘야지
            var result = await controller.CommentCreate(
                    id : id,
                    Username: Username,
                    Comment: Comment
                );

            //assert로 값 비교
            Assert.IsType<RedirectToActionResult>(result);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Details", redirectResult.ActionName);
            Assert.Equal(id, redirectResult.RouteValues["id"]);

            
            

        }
    }
}
