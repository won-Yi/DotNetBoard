using Board.Controllers;
using Board.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Microsoft.AspNetCore.Http;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;

namespace BoardTests.CreateTests
{
    public class CreateTests
    {

        [Theory]
        [InlineData("title", "content", "user", "category")]
        [InlineData("title", null, "user", "category")]
        [InlineData("title", "content", null, "category")]
        [InlineData("title", "content", "user", null)]

        //Arrange
        public async Task Create_ReturnsBadRequest_WhenRequiredFieldsAreMissing(
                        string title,
                        string content,
                        string username,
                        string category
                        )    
        {
            var options = new DbContextOptionsBuilder<BoardContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;
            var context = new BoardContext(options);
            var env = new Mock<IWebHostEnvironment>().Object;
            var controller = new NoticesController(context, env);

            var result = await controller.Create(
                title: title,
                content: content,
                username: username,
                category: category,
                files:null
                );

            result.As<ViewResult>().Model.Should().BeNull();
            result.Should().BeOfType<ViewResult>().Which.Should().NotBeNull();
        }
    }
}
