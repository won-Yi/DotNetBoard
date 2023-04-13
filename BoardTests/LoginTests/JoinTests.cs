using Board.Controllers;
using Board.Data;
using Board.Views.User;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardTests.LoginTests

{
    using Board.Models;
    using Newtonsoft.Json.Linq;
    using FluentAssertions;

    public class JoinTests
    {
        [Theory]
        [InlineData("won@naver.com", "7009900", "username")]
        //[InlineData("email2@naver.com", "1234")]
        public async Task Join_ReturnsBadRequest_WhenRequiredFieldsAreMissing(
                        string email,
                        string password,
                        string username
                        )
        {
            var options = new DbContextOptionsBuilder<BoardContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;
            var context = new BoardContext(options);
            var controller = new UserController(context);

            var result = await controller.Join(email, password, username);

            //Null이면 에러
            result.Should().NotBeNull();

        }

        public async Task Join_ReturnsBadRequest_WhenExistEmail(
                        string email,
                        string password,
                        string username
                        )
        {
            var options = new DbContextOptionsBuilder<BoardContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;
            var context = new BoardContext(options);
           
            var user = new User
            {
                Email = email,
                HashPassword = password,
                UserNickName = username
            };
            context.User.Add(user);
            context.SaveChanges();

            var controller = new UserController(context);

            var result = await controller.Join(email, password, username);

            //email중복에러
            result.Should().BeOfType<BadRequestObjectResult>();

            // 중복된 이메일을 처리하지 않는 경우
            var result2 = await controller.Join("test@test.com", "1234", "test");
            result2.Should().BeOfType<OkResult>();

        }//emial이 이미 존재하는지

    }

}




