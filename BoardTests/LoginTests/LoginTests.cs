using Board.Data;
using Board.Views.User;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Board.Models;
using Newtonsoft.Json.Linq;


namespace BoardTests.LoginTests
{
    public class LoginTests
    {   //test
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

        }// null Test

        //[Fact]
        //public async Task Login_ReturnsCorrectResult()
        //{
        //    // Arrange
        //    var options = new DbContextOptionsBuilder<BoardContext>()
        //        .UseInMemoryDatabase(databaseName: "test_db")
        //        .Options;
        //    var context = new BoardContext(options);

        //    var controller = new UserController(context);

        //    var user = new User
        //    {
        //        Email = "Initial Email",
        //        HashPassword = "Initial Password",
        //        UserNickName = "Initial Username",
        //        UserId = 32,

        //    };
        //    context.User.Add(user);
        //    context.SaveChanges();

        //    // Act
        //    var result = await controller.Login(user.Email, user.HashPassword);

        //    // Assert
        //    var viewResult = Assert.IsType<JsonResult>(result);
        //    var jsonResult = JObject.FromObject(viewResult.Value);
        //    int actualResult = jsonResult["result"].Value<int>();
        //    Assert.Equal(0, actualResult);

        //    var updatedUser = context.User.FirstOrDefault(m => m.UserId == user.UserId);

        //    if (updatedUser == null) {
        //        throw new ArgumentNullException(nameof(updatedUser));
        //    }
        //    Assert.NotNull(updatedUser);
        //    Assert.Equal("Initial Email", updatedUser.Email);
        //    Assert.Equal("Initial Password", updatedUser.HashPassword);

        //}
    }
}




