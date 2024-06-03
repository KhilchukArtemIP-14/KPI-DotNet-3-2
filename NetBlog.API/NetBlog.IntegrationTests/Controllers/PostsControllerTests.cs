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
    public class PostsControllerTests:IClassFixture<AppFactory>
    {
        private readonly HttpClient _client;
        private readonly AppFactory _factory;

        public PostsControllerTests(AppFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task AddPost_WhenAuthorizedAsAuthorAndValidData_ShouldReturnOk()
        {
            await _factory.SeedDataAsync();
            var jwtToken = await JWTGenerator.GetExistingAuthorJwt(_client);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var createPostDto = new CreatePostDTO
            {
                Title = "newpost",
                ContentPreview = "newpreview",
                Content = "newcontent",
                CreatedBy = "3c8b0d12-13e9-4f42-85a4-5d3ce1e7e34f"
            };
            var content = new StringContent(JsonConvert.SerializeObject(createPostDto), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/Posts", content);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var createdPost = JsonConvert.DeserializeObject<PostDTO>(responseString);

            Assert.NotNull(createdPost);
            Assert.Equal(createPostDto.Title, createdPost.Title);
            Assert.Equal(createPostDto.ContentPreview, createdPost.ContentPreview);
            Assert.Equal(createPostDto.Content, createdPost.Content);
        }
        [Fact]
        public async Task AddPost_WhenAuthorizedAsAuthorAndInvalidData_ShouldReturnBadRequest()
        {
            await _factory.SeedDataAsync();
            var jwtToken = await JWTGenerator.GetExistingAuthorJwt(_client);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var createPostDto = new CreatePostDTO();
            var content = new StringContent(JsonConvert.SerializeObject(createPostDto), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/Posts", content);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task AddPost_WhenUnauthorized_ShouldReturnForbidden()
        {
            await _factory.SeedDataAsync();
            var jwtToken = await JWTGenerator.GetExistingReaderJwt(_client);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var createPostDto = new CreatePostDTO
            {
                Title = "newpost",
                ContentPreview = "newpreview",
                Content = "newcontent",
                CreatedBy = "4c8b0d12-13e9-4f42-85a4-5d3ce1e7e34f"
            };
            var content = new StringContent(JsonConvert.SerializeObject(createPostDto), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/Posts", content);

            Assert.Equal(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task DeletePost_WhenAuthorizedAndPostExists_ShouldReturnOk()
        {
            await _factory.SeedDataAsync();
            var jwtToken = await JWTGenerator.GetExistingAuthorJwt(_client);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var postId = new Guid("f6536d0a-7e56-4a1e-9278-f1d5cc95e0b2");
            var response = await _client.DeleteAsync($"/api/Posts/{postId}");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var deletedPost = JsonConvert.DeserializeObject<PostDTO>(responseString);

            Assert.NotNull(deletedPost);
            Assert.Equal(postId, deletedPost.Id);
        }

        [Fact]
        public async Task DeletePost_WhenUnauthorized_ShouldReturnForbidden()
        {
            await _factory.SeedDataAsync();
            var jwtToken = await JWTGenerator.GetExistingReaderJwt(_client);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var postId = new Guid("f6536d0a-7e56-4a1e-9278-f1d5cc95e0b2");
            var response = await _client.DeleteAsync($"/api/Posts/{postId}");

            Assert.Equal(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task DeletePost_WhenPostDoesNotExist_ShouldReturnNotFound()
        {
            await _factory.SeedDataAsync();
            var jwtToken = await JWTGenerator.GetExistingAuthorJwt(_client);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var postId = Guid.NewGuid();
            var response = await _client.DeleteAsync($"/api/Posts/{postId}");

            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetPostById_WhenPostExists_ShouldReturnOk()
        {
            await _factory.SeedDataAsync();
            var postId = new Guid("f6536d0a-7e56-4a1e-9278-f1d5cc95e0b2");

            var response = await _client.GetAsync($"/api/Posts/{postId}");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var post = JsonConvert.DeserializeObject<PostDTO>(responseString);

            Assert.NotNull(post);
            Assert.Equal(postId, post.Id);
        }

        [Fact]
        public async Task GetPostById_WhenPostDoesNotExist_ShouldReturnNotFound()
        {
            await _factory.SeedDataAsync();
            var postId = Guid.NewGuid();

            var response = await _client.GetAsync($"/api/Posts/{postId}");

            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetPostShortcutsOfUser_WhenPostsExist_ShouldReturnOk()
        {
            await _factory.SeedDataAsync();
            var userId = "3c8b0d12-13e9-4f42-85a4-5d3ce1e7e34f";

            var response = await _client.GetAsync($"/api/Posts/user/{userId}?pageNumber=1&pageSize=2");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var postShortcuts = JsonConvert.DeserializeObject<List<PostShortcutDTO>>(responseString);

            Assert.NotNull(postShortcuts);
            Assert.True(postShortcuts.Count > 0);
        }

        [Fact]
        public async Task GetPostSummaries_ShouldReturnOk()
        {
            await _factory.SeedDataAsync();

            var response = await _client.GetAsync("/api/Posts?pageNumber=1&pageSize=2");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var postSummaries = JsonConvert.DeserializeObject<List<PostSummaryDTO>>(responseString);

            Assert.NotNull(postSummaries);
            Assert.True(postSummaries.Count > 0);
        }

        [Fact]
        public async Task UpdatePost_WhenAuthorizedAndValidData_ShouldReturnOk()
        {
            await _factory.SeedDataAsync();
            var jwtToken = await JWTGenerator.GetExistingAuthorJwt(_client);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var postId = new Guid("f6536d0a-7e56-4a1e-9278-f1d5cc95e0b2");
            var updatePostDto = new UpdatePostDTO
            {
                Title = "post123",
                ContentPreview = "preview123",
                Content = "content123"
            };
            var content = new StringContent(JsonConvert.SerializeObject(updatePostDto), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"/api/Posts/{postId}", content);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var updatedPost = JsonConvert.DeserializeObject<PostDTO>(responseString);

            Assert.NotNull(updatedPost);
            Assert.Equal(postId, updatedPost.Id);
            Assert.Equal(updatePostDto.Title, updatedPost.Title);
            Assert.Equal(updatePostDto.ContentPreview, updatedPost.ContentPreview);
        }
        [Fact]
        public async Task UpdatePost_WhenAuthorizedAndInvalidData_ShouldReturnBadRequest()
        {
            await _factory.SeedDataAsync();
            var jwtToken = await JWTGenerator.GetExistingAuthorJwt(_client);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var postId = new Guid("f6536d0a-7e56-4a1e-9278-f1d5cc95e0b2");
            var updatePostDto = new UpdatePostDTO();
            var content = new StringContent(JsonConvert.SerializeObject(updatePostDto), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"/api/Posts/{postId}", content);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task UpdatePost_WhenUnauthorized_ShouldReturnForbidden()
        {
            await _factory.SeedDataAsync();
            var jwtToken = await JWTGenerator.GetExistingReaderJwt(_client);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var postId = new Guid("f6536d0a-7e56-4a1e-9278-f1d5cc95e0b2");
            var updatePostDto = new UpdatePostDTO
            {
                Title = "post123",
                ContentPreview = "preview123",
                Content = "content123"
            };
            var content = new StringContent(JsonConvert.SerializeObject(updatePostDto), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"/api/Posts/{postId}", content);

            Assert.Equal(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task UpdatePost_WhenPostDoesNotExist_ShouldReturnNotFound()
        {
            await _factory.SeedDataAsync();
            var jwtToken = await JWTGenerator.GetExistingAuthorJwt(_client);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var postId = Guid.NewGuid();
            var updatePostDto = new UpdatePostDTO
            {
                Title = "post123",
                ContentPreview = "preview123",
                Content = "content123"
            };
            var content = new StringContent(JsonConvert.SerializeObject(updatePostDto), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"/api/Posts/{postId}", content);

            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
