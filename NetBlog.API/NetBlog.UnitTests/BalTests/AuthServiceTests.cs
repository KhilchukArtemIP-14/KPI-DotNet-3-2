using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Moq;
using NetBlog.BAL.Services.AuthServices;
using NetBlog.BAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.UnitTests.BalTests
{
    public class AuthServiceTests
    {
        private Mock<UserManager<IdentityUser>> _userManagerMock;
        private IConfiguration _configMock;
        private AuthService _authService;
        public AuthServiceTests() { 
            _userManagerMock = MockUserManager.CreateMockUserManager();
            _configMock = MockConfiguration.CreateMockConfiguration();
            _authService = new AuthService(_userManagerMock.Object, _configMock);
        }
        [Fact]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            var loginUserDTO = new LoginUserDTO { Email = "oof1@gmail.com", Password = "TestPassword" };

            var result = await _authService.Login(loginUserDTO);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsErrorResponse()
        {
            var loginUserDTO = new LoginUserDTO { Email = "oof5@gmail.com", Password = "InvalidPassword" };

            var result = await _authService.Login(loginUserDTO);

            Assert.Null(result);
        }

        [Fact]
        public async Task Register_ValidUser_ReturnsSuccessResponse()
        {
            var registerUserDTO = new RegisterUserDTO
            {
                Name = "NewUser",
                Email = "newuser@example.com",
                Password = "TestPassword",
                Roles = new string[] { "Author" }
            };

            var result = await _authService.Register(registerUserDTO);

            Assert.NotNull(result);
            Assert.True(result.Succeeded);
        }

        [Fact]
        public async Task Register_InvalidUser_ReturnsErrorResponse()
        {
            _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Registration failed" }));
            var registerUserDTO = new RegisterUserDTO
            {
                Name = "InvalidUser",
                Email = "invaliduser@example.com",
                Password = "InvalidPassword"
            };

            var result = await _authService.Register(registerUserDTO);

            Assert.Null(result);
        }
    }
}
