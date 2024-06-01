using Microsoft.AspNetCore.Http.HttpResults;
using NetBlog.DAL.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Security.Policy;
using NetBlog.EndToEndTests.Utilities;
using NetBlog.BAL.DTO;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Xml.Linq;

namespace NetBlog.EndToEndTests
{
    public class Scenarios : IClassFixture<AppFactory>
    {
        private readonly HttpClient _client;
        private readonly AppFactory _factory;

        public Scenarios(AppFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task AuthorFullPostLifecycleScenarioAsync()
        {
            //scenario:
            //1.Author logs in
            //2.Author Creates a post
            //3.Author reads this post
            //4.Author scrolls comments for post
            //5.Author deletes one comment
            //6.Author updates post
            //7.Author deletes post

            await _factory.SeedDataAsync();
            //Author logs in
            var loginDto = new LoginUserDTO
            {
                Email = "frookt4555@gmail.com",
                Password = "SuperSecurePaswwordqwerty@"
            };
            var content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/Auth/login", content);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var loginResponse = JsonConvert.DeserializeObject<LoginResponseDTO>(responseString);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse.Token);

            //Author creates a post
            var createPostDto = new CreatePostDTO
            {
                Title = "Top 10 wiseposting quotes",
                Content = "never gonna give you up",
                ContentPreview = "hmmm, yes very wise",
                CreatedBy = "3c8b0d12-13e9-4f42-85a4-5d3ce1e7e34f"
            };
            var createPostContent = new StringContent(JsonConvert.SerializeObject(createPostDto), Encoding.UTF8, "application/json");

            var createPostResponse = await _client.PostAsync("/api/Posts", createPostContent);
            createPostResponse.EnsureSuccessStatusCode();
            var createPostResponseString = await createPostResponse.Content.ReadAsStringAsync();
            var createdPost = JsonConvert.DeserializeObject<PostDTO>(createPostResponseString);

            Assert.NotNull(createdPost);
            Assert.Equal(createPostDto.Title, createdPost.Title);
            Assert.Equal(createPostDto.Content, createdPost.Content);
            Assert.Equal(createPostDto.ContentPreview, createdPost.ContentPreview);
            Assert.Equal("3c8b0d12-13e9-4f42-85a4-5d3ce1e7e34f", createdPost.CreatedBy.UserId);

            //Author reads this post
            var readPostResponse = await _client.GetAsync($"/api/Posts/{createdPost.Id}?commentsToLoad=1");

            readPostResponse.EnsureSuccessStatusCode();
            var readPostResponseString = await readPostResponse.Content.ReadAsStringAsync();
            var readPost = JsonConvert.DeserializeObject<PostDTO>(readPostResponseString);

            Assert.NotNull(readPost);
            Assert.Equal(createdPost.Id, readPost.Id);
            Assert.Equal(createPostDto.Title, readPost.Title);
            Assert.Equal(createPostDto.Content, readPost.Content);
            Assert.Equal(createPostDto.ContentPreview, readPost.ContentPreview);
            Assert.Equal("3c8b0d12-13e9-4f42-85a4-5d3ce1e7e34f", readPost.CreatedBy.UserId);
            Assert.Equal("Author", readPost.CreatedBy.UserName);
            Assert.NotNull(readPost.Comments);
            Assert.Equal(readPost.Comments.Count(), 0);

            // Author scrolls comments for his another post
            var getCommentsResponse = await _client.GetAsync($"/api/Comments/post/f6536d0a-7e56-4a1e-9278-f1d5cc95e0b2?pageNumber=2&pageSize=1&orderByDateAscending=true");
            getCommentsResponse.EnsureSuccessStatusCode();
            var getCommentsResponseString = await getCommentsResponse.Content.ReadAsStringAsync();
            var comments = JsonConvert.DeserializeObject<List<CommentDTO>>(getCommentsResponseString);
            Assert.NotNull(comments);
            Assert.Equal(comments.Count(),1);
            Assert.Equal(comments[0].CreatedBy.UserId, "4c8b0d12-13e9-4f42-85a4-5d3ce1e7e34f");
            Assert.Equal(comments[0].CreatedBy.UserName, "Reader");
            Assert.Equal(comments[0].CommentText, "Great job!");
            Assert.Equal(comments[0].Id, Guid.Parse("69a06e65-7598-4dbf-99cd-218ab5975456"));

            //Author deletes one comment
            if (comments.Count > 0)
            {
                var commentToDelete = comments[0];
                var deleteCommentResponse = await _client.DeleteAsync($"/api/Comments/{commentToDelete.Id}");
                deleteCommentResponse.EnsureSuccessStatusCode();
                var deleteCommentResponseString = await deleteCommentResponse.Content.ReadAsStringAsync();
                var deletedComment = JsonConvert.DeserializeObject<CommentDTO>(deleteCommentResponseString);
                Assert.NotNull(deletedComment);
                Assert.Equal(commentToDelete.Id, deletedComment.Id);

                // assure it was deleted
                var commentsAfterDeleteRequest = await _client.GetAsync($"/api/Comments/post/f6536d0a-7e56-4a1e-9278-f1d5cc95e0b2?pageNumber=2&pageSize=1");
                commentsAfterDeleteRequest.EnsureSuccessStatusCode();
                var getCommentsAfterDeleteResponseString = await commentsAfterDeleteRequest.Content.ReadAsStringAsync();
                var commentsAfterDelete = JsonConvert.DeserializeObject<List<CommentDTO>>(getCommentsAfterDeleteResponseString);
                Assert.NotNull(commentsAfterDelete);
                Assert.Empty(commentsAfterDelete);
            }

            //Author updates the post
            var updatePostDto = new UpdatePostDTO
            {
                Title = "Updated top 10 wiseposting quotes",
                Content = "Updated never gonna give you up",
                ContentPreview = "hmmm, yes now very wiser"
            };
            var updatePostContent = new StringContent(JsonConvert.SerializeObject(updatePostDto), Encoding.UTF8, "application/json");
            var updatePostResponse = await _client.PutAsync($"/api/Posts/{createdPost.Id}", updatePostContent);
            updatePostResponse.EnsureSuccessStatusCode();
            var updatePostResponseString = await updatePostResponse.Content.ReadAsStringAsync();
            var updatedPost = JsonConvert.DeserializeObject<PostDTO>(updatePostResponseString);
            Assert.NotNull(updatedPost);
            Assert.Equal(updatePostDto.Title, updatedPost.Title);
            Assert.Equal(updatePostDto.Content, updatedPost.Content);
            Assert.Equal(updatePostDto.ContentPreview, updatedPost.ContentPreview);
            Assert.Equal("3c8b0d12-13e9-4f42-85a4-5d3ce1e7e34f", updatedPost.CreatedBy.UserId);
        }

        [Fact]
        public async Task UnregisteredUserFullCommentingLifecycleScenarioAsync()
        {
            // Scenario:
            // 1.Unregistered user checks post
            // 2.Unregistered user registers
            // 3.Registered user logs in
            // 4.Authenticated user adds comment to post
            // 5.Authenticated user checks his user summary
            // 6.Authenticated user checks his comments
            // 7.Authenticated user deletes his comment

            await _factory.SeedDataAsync();

            //Unregistered user checks post
            var postId = "f6536d0a-7e56-4a1e-9278-f1d5cc95e0b2";
            var checkPostResponse = await _client.GetAsync($"/api/Posts/{postId}?commentsToLoad=1");
            checkPostResponse.EnsureSuccessStatusCode();
            var checkPostResponseString = await checkPostResponse.Content.ReadAsStringAsync();
            var checkedPost = JsonConvert.DeserializeObject<PostDTO>(checkPostResponseString);
            Assert.NotNull(checkedPost);

            //Unregistered user registers
            var registerDto = new RegisterUserDTO
            {
                Name = "WiseUser",
                Email = "wiseuser@wiseposters.com",
                Password = "SuperSecurePassword@123",
                Roles = ["Reader"]
            };
            var registerContent = new StringContent(JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json");

            var registerResponse = await _client.PostAsync("/api/Auth/register", registerContent);
            registerResponse.EnsureSuccessStatusCode();

            //Registered user logs in
            var loginDto = new LoginUserDTO
            {
                Email = "wiseuser@wiseposters.com",
                Password = "SuperSecurePassword@123"
            };
            var loginContent = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");

            var loginResponse = await _client.PostAsync("/api/Auth/login", loginContent);

            loginResponse.EnsureSuccessStatusCode();
            var loginResponseString = await loginResponse.Content.ReadAsStringAsync();
            var loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDTO>(loginResponseString);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponseDto.Token);

            // Logged user adds comment to post
            var createCommentDto = new CreateCommentDTO
            {
                PostId = Guid.Parse(postId),
                AuthorId = loginResponseDto.UserId,
                CommentText = "This is a wise comment."
            };
            var createCommentContent = new StringContent(JsonConvert.SerializeObject(createCommentDto), Encoding.UTF8, "application/json");

            var createCommentResponse = await _client.PostAsync("/api/Comments", createCommentContent);
            createCommentResponse.EnsureSuccessStatusCode();
            var createCommentResponseString = await createCommentResponse.Content.ReadAsStringAsync();
            var createdComment = JsonConvert.DeserializeObject<CommentDTO>(createCommentResponseString);

            Assert.NotNull(createdComment);
            Assert.Equal(createCommentDto.CommentText, createdComment.CommentText);
            Assert.Equal(createCommentDto.AuthorId, createdComment.CreatedBy.UserId);

            // Logged user checks his user summary
            var userSummaryResponse = await _client.GetAsync($"/api/UserSummary/{loginResponseDto.UserId}");
            userSummaryResponse.EnsureSuccessStatusCode();
            var userSummaryResponseString = await userSummaryResponse.Content.ReadAsStringAsync();
            var userSummary = JsonConvert.DeserializeObject<UserSummaryDTO>(userSummaryResponseString);
            Assert.NotNull(userSummary);
            Assert.Equal(loginResponseDto.UserId, userSummary.Id);
            Assert.Equal(registerDto.Name, userSummary.Name);
            Assert.Equal(registerDto.Email, userSummary.Email);
            Assert.Equal("Empty", userSummary.Bio);

            // Logged user checks his comments
            var userCommentsResponse = await _client.GetAsync($"/api/Comments/user/{loginResponseDto.UserId}?pageNumber=1&pageSize=5");
            userCommentsResponse.EnsureSuccessStatusCode();
            var userCommentsResponseString = await userCommentsResponse.Content.ReadAsStringAsync();
            var userComments = JsonConvert.DeserializeObject<List<CommentShortcutDTO>>(userCommentsResponseString);
            Assert.NotNull(userComments);
            Assert.True(userComments.Count > 0);
            var userComment = userComments[0];
            Assert.Equal(createdComment.CommentText, userComment.CommentText);

            // Logged user deletes his comment
            var deleteCommentResponse = await _client.DeleteAsync($"/api/Comments/{createdComment.Id}");
            deleteCommentResponse.EnsureSuccessStatusCode();
            var deleteCommentResponseString = await deleteCommentResponse.Content.ReadAsStringAsync();
            var deletedComment = JsonConvert.DeserializeObject<CommentDTO>(deleteCommentResponseString);
            Assert.NotNull(deletedComment);
            Assert.Equal(createdComment.Id, deletedComment.Id);
            // assure it was deleted
            userCommentsResponse = await _client.GetAsync($"/api/Comments/user/{loginResponseDto.UserId}?pageNumber=1&pageSize=5");
            userCommentsResponse.EnsureSuccessStatusCode();

            userCommentsResponseString = await userCommentsResponse.Content.ReadAsStringAsync();

            userComments = JsonConvert.DeserializeObject<List<CommentShortcutDTO>>(userCommentsResponseString);
            Assert.NotNull(userComments);
            Assert.Equal(userComments.Count, 0);
        }
    }
}