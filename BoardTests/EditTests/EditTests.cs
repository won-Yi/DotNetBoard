//using Board.Controllers;
//using Board.Data;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Options;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Moq;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Mvc;
//using Xunit;
//using Microsoft.AspNetCore.Http;
//using FluentAssertions;
//using System.Diagnostics.CodeAnalysis;
//using Board.Models;
//using System.Diagnostics;
//using Newtonsoft.Json.Linq;
//using Board.Infrastructure;
//using NuGet.Protocol.Core.Types;
//using System.Reflection;
//using Dropbox.Api.TeamLog;

//namespace BoardTests.CreateTests
//{
//    public class EditTests
//    {

//        [Theory]
//        [InlineData(56,"title", "content", "user", "category")]
//        //[InlineData(2,"title", null, "user", "category")]
//        //[InlineData(3,"title", "content", null, "category")]
//        //[InlineData(4,"title", "content", "user", null)]

//        public async Task Edit_ReturnsBadRequest_WhenRequiredFieldsAreMissing(
//                        int id,
//                        string title,
//                        string content,
//                        string username,
//                        string category
//                        )
//        {
//            var options = new DbContextOptionsBuilder<BoardContext>()
//               .UseInMemoryDatabase(databaseName: "test_db")
//               .Options;
//            var context = new BoardContext(options);
//            var env = new Mock<IWebHostEnvironment>().Object;
//            var _context = new EFSboardSessionRepository(context);
//            var controller = new NoticesController(_context,env, context);

//            var result = await controller.Edit(id, title, content, username, category);


//            Assert.NotNull(result);
//        }//null값을 확인하는 테스트




        //[Fact]
        //public async Task Edit_ReturnsCorrectResult()
        //{
        //    // Arrange
        //    var options = new DbContextOptionsBuilder<BoardContext>()
        //       .UseInMemoryDatabase(databaseName: "test_db")
        //       .Options;
        //    var context = new BoardContext(options);
        //    var env = new Mock<IWebHostEnvironment>().Object;
        //    var _context = new EFSboardSessionRepository(context);
        //    var controller = new NoticesController(_context, env, context);

        //    var notices = new Notice
        //    {
        //        Id = 56,
        //        Title = "Initial Title",
        //        Content = "Initial Content",
        //        UserName = "Initial User",
        //        Category = "Initial Category"
        //    };


        //    var result = await controller.Edit(
        //       id: notices.Id,
        //       title: "New Title",
        //       content: "New Content",
        //       username: "New User",
        //       category: "New Category"
        //       );

        //    Assert.Equal("New Title", notices.Title);
        //    Assert.Equal("New Content", notices.Content);
        //    Assert.Equal("New User", notices.UserName);
        //    Assert.Equal("New Category", notices.Category);
        //}//전달받은 값과 return값이 같은지 비교


//    }//class
//}//nameSpace
