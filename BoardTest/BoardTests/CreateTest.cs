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
//using Board.Infrastructure;
//using Board.Interfaces;
//using static Dropbox.Api.Sharing.ListFileMembersIndividualResult;

//namespace BoardTest.BoardTests
//{

//    public class CreateTest
//    {
//        [Theory]
//        [InlineData("title", "content", "user", "category")]
//        //[InlineData("title", null, "user", "category")]
//        //[InlineData("title", "content", null, "category")]
//        //[InlineData("title", "content", "user", null)]

//        //Arrange
//        public async Task Create_ReturnsBadRequest_WhenRequiredFieldsAreMissing(
//                        string title,
//                        string content,
//                        string username,
//                        string category
//                        )
//        {
//            var options = new DbContextOptionsBuilder<BoardContext>()
//            .UseSqlServer("Data Source =.\\SQLEXPRESS; Initial Catalog = board; Persist Security Info = True; User ID = sa; Password = 123qwe!@#QWE;Trusted_Connection=True;MultipleActiveResultSets=true;Trust Server Certificate=true")
//            .Options;
//            var context = new BoardContext(options);
//            var env = new Mock<IWebHostEnvironment>().Object;
//            var createSessionRepo = new EFSboardSessionRepository(context); // 구현한 클래스의 인스턴스 생성
//            var controller = new NoticesController(createSessionRepo, env, context); //

//            var result = await controller.Create(
//                 title: title,
//                 content: content,
//                 username: username,
//                 category: category,
//                 files: new List<IFormFile>()
//                 );

//            Assert.NotNull(result);
//            //Assert.Equal("New Title", repository.Title);
//            //Assert.Equal("New Content", repository.Content);
//            //Assert.Equal("New User", repository.UserName);
//            //Assert.Equal("New Category", repository.Category);


//        }
//    }
//}