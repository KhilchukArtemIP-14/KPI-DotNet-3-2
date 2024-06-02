using NetBlog.Application.DTOs;
using NetBlog.IntegrationTests.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.IntegrationTests.Controllers
{
    public class UserSummaryControllerTests : IClassFixture<AppFactory>
    {
        private readonly HttpClient _client;
        private readonly AppFactory _factory;

        public UserSummaryControllerTests(AppFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task GetUserSummary_WhenUserExists_ShouldReturnOk()
        {
            await _factory.SeedDataAsync();
            var userId = "3c8b0d12-13e9-4f42-85a4-5d3ce1e7e34f";

            var response = await _client.GetAsync($"/api/UserSummary/{userId}");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var userSummary = JsonConvert.DeserializeObject<UserSummaryDTO>(responseString);

            Assert.NotNull(userSummary);
            Assert.Equal(userId, userSummary.Id);
            Assert.Equal("Author", userSummary.Name);
            Assert.Equal("frookt4555@gmail.com", userSummary.Email);
            Assert.Contains("Author", userSummary.Roles);
            Assert.Equal("Empty", userSummary.Bio);
        }

        [Fact]
        public async Task GetUserSummary_WhenUserDoesNotExist_ShouldReturnNotFound()
        {
            var userId = "asdasdasdasdasdasdasdasdas";

            var response = await _client.GetAsync($"/api/UserSummary/{userId}");

            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateUserSummary_WhenValidDataAndAuthorized_ShouldReturnOk()
        {
            await _factory.SeedDataAsync();
            var jwtToken = await JWTGenerator.GetExistingAuthorJwt(_client);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var userId = "3c8b0d12-13e9-4f42-85a4-5d3ce1e7e34f";
            var updateUserDto = new UpdateUserDTO
            {
                Name = "UpdatedAuthor",
                Bio = "Updated bio"
            };
            var content = new StringContent(JsonConvert.SerializeObject(updateUserDto), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"/api/UserSummary/{userId}", content);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var updatedSummary = JsonConvert.DeserializeObject<UserSummaryDTO>(responseString);

            Assert.NotNull(updatedSummary);
            Assert.Equal(userId, updatedSummary.Id);
            Assert.Equal("UpdatedAuthor", updatedSummary.Name);
            Assert.Equal("frookt4555@gmail.com", updatedSummary.Email);
            Assert.Contains("Author", updatedSummary.Roles);
            Assert.Equal("Updated bio", updatedSummary.Bio);
        }

        [Fact]
        public async Task UpdateUserSummary_WhenInvalidDataAndAuthorized_ShouldReturnBadRequest()
        {
            await _factory.SeedDataAsync();
            var userId = "3c8b0d12-13e9-4f42-85a4-5d3ce1e7e34f";
            var jwtToken = await JWTGenerator.GetExistingAuthorJwt(_client);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            var updateUserDto = new UpdateUserDTO
            {
                Name = null, 
                Bio = "Updated bio"
            };
            var content = new StringContent(JsonConvert.SerializeObject(updateUserDto), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"/api/UserSummary/{userId}", content);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
