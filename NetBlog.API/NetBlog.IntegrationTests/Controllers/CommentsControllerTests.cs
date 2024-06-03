using NetBlog.BAL.DTO;
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
    public class CommentsControllerTests : IClassFixture<AppFactory>
    {
        private readonly HttpClient _client;
        private readonly AppFactory _factory;

        public CommentsControllerTests(AppFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateComment_WhenAuthorizedAndValidData_ShouldReturnOk()
        {
            await _factory.SeedDataAsync();
            var jwtToken = await JWTGenerator.GetExistingAuthorJwt(_client);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var createCommentDto = new CreateCommentDTO
            {
                PostId = new Guid("f6536d0a-7e56-4a1e-9278-f1d5cc95e0b2"),
                AuthorId = "3c8b0d12-13e9-4f42-85a4-5d3ce1e7e34f",
                CommentText = "This is a comment."
            };
            var content = new StringContent(JsonConvert.SerializeObject(createCommentDto), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/Comments", content);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var createdComment = JsonConvert.DeserializeObject<CommentDTO>(responseString);

            Assert.NotNull(createdComment);
            Assert.Equal("This is a comment.", createdComment.CommentText);
        }
        [Fact]
        public async Task CreateComment_WhenAuthorizedAndInvalidData_ShouldReturnBadRequest()
        {
            await _factory.SeedDataAsync();
            var jwtToken = await JWTGenerator.GetExistingAuthorJwt(_client);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var createCommentDto = new CreateCommentDTO();
        
            var content = new StringContent(JsonConvert.SerializeObject(createCommentDto), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/Comments", content);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task CreateComment_WhenUnauthorized_ShouldReturnUnauthorized()
        {
            await _factory.SeedDataAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "invalid_token");

            var createCommentDto = new CreateCommentDTO
            {
                PostId = new Guid("f6536d0a-7e56-4a1e-9278-f1d5cc95e0b2"),
                AuthorId = "4c8b0d12-13e9-4f42-85a4-5d3ce1e7e34f",
                CommentText = "This is a comment."
            };
            var content = new StringContent(JsonConvert.SerializeObject(createCommentDto), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/Comments", content);

            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task DeleteComment_WhenAuthorizedAndCommentExists_ShouldReturnOk()
        {
            await _factory.SeedDataAsync();
            var jwtToken = await JWTGenerator.GetExistingAuthorJwt(_client);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var commentId = new Guid("3c724b9c-0452-452e-a738-cfd52e586b39");
            var response = await _client.DeleteAsync($"/api/Comments/{commentId}");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var deletedComment = JsonConvert.DeserializeObject<CommentDTO>(responseString);

            Assert.NotNull(deletedComment);
            Assert.Equal(commentId, deletedComment.Id);
        }

        [Fact]
        public async Task DeleteComment_WhenUnauthorized_ShouldReturnForbidden()
        {
            await _factory.SeedDataAsync();
            var jwtToken = await JWTGenerator.GetExistingReaderJwt(_client);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var commentId = new Guid("3c8b0d12-13e9-4f42-85a4-5d3ce1e7e34f");
            var response = await _client.DeleteAsync($"/api/Comments/{commentId}");

            Assert.Equal(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task DeleteComment_WhenCommentDoesNotExist_ShouldReturnNotFound()
        {
            await _factory.SeedDataAsync();
            var jwtToken = await JWTGenerator.GetExistingAuthorJwt(_client);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var commentId = Guid.NewGuid();
            var response = await _client.DeleteAsync($"/api/Comments/{commentId}");

            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetCommentsForPost_WhenCommentsExist_ShouldReturnOk()
        {
            await _factory.SeedDataAsync();
            var postId = new Guid("f6536d0a-7e56-4a1e-9278-f1d5cc95e0b2");

            var response = await _client.GetAsync($"/api/Comments/post/{postId}?pageNumber=1&pageSize=2");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var comments = JsonConvert.DeserializeObject<List<CommentDTO>>(responseString);

            Assert.NotNull(comments);
            Assert.True(comments.Count > 0);
        }

        [Fact]
        public async Task GetCommentsForPost_WhenPostDoesNotExist_ShouldReturnOk()
        {
            await _factory.SeedDataAsync();
            var postId = Guid.NewGuid();

            var response = await _client.GetAsync($"/api/Comments/post/{postId}?pageNumber=1&pageSize=2");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var comments = JsonConvert.DeserializeObject<List<CommentDTO>>(responseString);

            Assert.NotNull(comments);
            Assert.True(comments.Count == 0);
        }

        [Fact]
        public async Task GetShortcutsForUser_WhenCommentsExist_ShouldReturnOk()
        {
            await _factory.SeedDataAsync();
            var userId = "3c8b0d12-13e9-4f42-85a4-5d3ce1e7e34f";

            var response = await _client.GetAsync($"/api/Comments/user/{userId}?pageNumber=1&pageSize=2");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var commentShortcuts = JsonConvert.DeserializeObject<List<CommentShortcutDTO>>(responseString);

            Assert.NotNull(commentShortcuts);
            Assert.True(commentShortcuts.Count > 0);
        }

        [Fact]
        public async Task GetShortcutsForUser_WhenUserDoesNotExist_ShouldReturnOk()
        {
            await _factory.SeedDataAsync();
            var userId = "nonexistent_user_id";

            var response = await _client.GetAsync($"/api/Comments/user/{userId}?pageNumber=1&pageSize=2");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var commentShortcuts = JsonConvert.DeserializeObject<List<CommentShortcutDTO>>(responseString);

            Assert.NotNull(commentShortcuts);
            Assert.True(commentShortcuts.Count == 0);
        }
    }
}
