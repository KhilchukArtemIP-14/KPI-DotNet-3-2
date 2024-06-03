using NetBlog.BAL.DTO;
using NetBlog.IntegrationTests.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.IntegrationTests.Controllers
{
    public class AuthControllerTests : IClassFixture<AppFactory>
    {
        private readonly HttpClient _client;
        private readonly AppFactory _factory;

        public AuthControllerTests(AppFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Register_WhenValidData_ShouldReturnOk()
        {
            await _factory.SeedDataAsync();
            var registerDto = new RegisterUserDTO
            {
                Name = "NewUser",
                Email = "newuser@example.com",
                Password = "SecurePassword@123",
                Roles = new[] { "Reader" }
            };
            var content = new StringContent(JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/Auth/register", content);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("User successfully created", responseString);
        }

        [Fact]
        public async Task Register_WhenInvalidData_ShouldReturnBadRequest()
        {
            var registerDto = new RegisterUserDTO
            {
                Name = "NewUser",
                Email = "invalid-email",
                Password = "short",
                Roles = new[] { "Reader" }
            };
            var content = new StringContent(JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/Auth/register", content);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Login_WhenValidCredentials_ShouldReturnOk()
        {
            await _factory.SeedDataAsync();
            var loginDto = new LoginUserDTO
            {
                Email = "frookt4444@gmail.com",
                Password = "SuperSecurePaswwordqwerty@"
            };
            var content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/Auth/login", content);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var loginResponse = JsonConvert.DeserializeObject<LoginResponseDTO>(responseString);

            Assert.NotNull(loginResponse);
            Assert.Equal("Reader", loginResponse.Name);
            Assert.Equal("frookt4444@gmail.com", loginResponse.Email);
            Assert.NotEmpty(loginResponse.Token);
            Assert.Contains("Reader", loginResponse.Roles);
        }

        [Fact]
        public async Task Login_WhenInvalidCredentials_ShouldReturnBadRequest()
        {
            var loginDto = new LoginUserDTO
            {
                Email = "frookt4444@gmail.com",
                Password = "WrongPassword"
            };
            var content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/Auth/login", content);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Couldn't login user", responseString);
        }
    }
}
