using BusinessLayer;
using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using WalletSystem.Controllers;

namespace WalletSystemTest
{
    public class UserControllerTest
    {
        private readonly UserController _controller;
        private readonly Mock<IUsers> _mockUsers;

        public UserControllerTest()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _mockUsers = new Mock<IUsers>();
            _controller = new UserController(configuration, _mockUsers.Object);
        }
        
        [Fact]
        public async Task Register_ReturnsSuccess_WhenRegistrationSuccesful()
        {
            var user = new UsersModelObject
            {
                Username = "testuser",
                Password = "testpassword"
            };

            _mockUsers.Setup(x => x.CheckUserExist(user)).ReturnsAsync(false);
            _mockUsers.Setup(x => x.Register(user)).ReturnsAsync(true);

            var result = await _controller.Register(user);

            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal("User registration successful!", okResult!.Value!.GetType().GetProperty("Message")!.GetValue(okResult.Value));
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenUsernameOrPasswordIsMissing()
        {
            var user = new UsersModelObject
            {
                Username = "",
                Password = ""
            };

            var result = await _controller.Register(user);

            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal("Username and password is required.", badRequestResult!.Value!.GetType().GetProperty("Message")!.GetValue(badRequestResult.Value));
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenUserAlreadyExists()
        {
            var user = new UsersModelObject
            {
                Username = "testuser",
                Password = "testpassword"
            };

            _mockUsers.Setup(x => x.CheckUserExist(user)).ReturnsAsync(true);

            var result = await _controller.Register(user);

            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal("Username already exists.", badRequestResult!.Value!.GetType().GetProperty("Message")!.GetValue(badRequestResult.Value));
        }
    }
}