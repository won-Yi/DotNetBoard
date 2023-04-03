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
using FluentAssertions;

namespace BoardTests.LoginTests
{
    public class LoginTests
    {
        [Theory]
        [InlineData("email@naver.com", "7009900")]
        //[InlineData("email2@naver.com", "1234")]
        public async Task Login_ReturnsBadRequest_WhenRequiredFieldsAreMissing(
                        string email,
                        string password
                        )
        {
            var options = new DbContextOptionsBuilder<BoardContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;
            var context = new BoardContext(options);
            var controller = new UserController(context);

            var result = await controller.Login(email, password);

            //Null이면 에러
            result.Should().NotBeNull();

        }
    }

}




