using AutoMapper;
using Moq;
using NetBlog.BAL.Services.PostsServices;
using NetBlog.BAL.Services.UserSummaryService;
using NetBlog.BAL.DTO;
using NetBlog.DAL.Models;
using NetBlog.DAL.Repositories;
using NetBlog.DAL.Specifications.Implementations;
using NetBlog.DAL.Specifications.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.UnitTests.BalTests
{
    public class PostServiceTests
    {
        private readonly Mock<IRepository<Post>> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserSummaryService> _userSummaryServiceMock;
        private readonly PostService _postService;

        public PostServiceTests()
        {
            _repositoryMock = new Mock<IRepository<Post>>();
            _mapperMock = new Mock<IMapper>();
            _userSummaryServiceMock = new Mock<IUserSummaryService>();
            _postService = new PostService(_repositoryMock.Object, _mapperMock.Object, _userSummaryServiceMock.Object);
        }

        [Fact]
        public async Task Add_ShouldReturnPostDTO()
        {
            var createPostDto = new CreatePostDTO { Title = "Test Title", Content = "Test Content", CreatedBy = "author1" };
            var post = new Post { Id = Guid.NewGuid(), Title = "Test Title", Content = "Test Content", CreatedBy = "author1" };
            var postDto = new PostDTO { Id = post.Id, Title = "Test Title", Content = "Test Content", CreatedBy = new UserShortcutDTO { UserId = "author1" } };

            _mapperMock.Setup(m => m.Map<Post>(createPostDto)).Returns(post);
            _repositoryMock.Setup(r => r.Add(post)).ReturnsAsync(post);
            _mapperMock.Setup(m => m.Map<PostDTO>(post)).Returns(postDto);

            var result = await _postService.Add(createPostDto);

            Assert.Equal(postDto.Id, result.Id);
            Assert.Equal(postDto.Title, result.Title);
            Assert.Equal(postDto.Content, result.Content);
            _repositoryMock.Verify(r => r.Add(It.IsAny<Post>()), Times.Once);
            _mapperMock.Verify(m => m.Map<PostDTO>(post), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldReturnPostDTO()
        {
            var postId = Guid.NewGuid();
            var post = new Post { Id = postId, Title = "Test Title", Content = "Test Content", CreatedBy = "author1" };
            var postDto = new PostDTO { Id = postId, Title = "Test Title", Content = "Test Content", CreatedBy = new UserShortcutDTO { UserId = "author1" } };

            _repositoryMock.Setup(r => r.Delete(postId)).ReturnsAsync(post);
            _mapperMock.Setup(m => m.Map<PostDTO>(post)).Returns(postDto);

            var result = await _postService.Delete(postId);

            Assert.Equal(postDto.Id, result.Id);
            Assert.Equal(postDto.Title, result.Title);
            Assert.Equal(postDto.Content, result.Content);
            _repositoryMock.Verify(r => r.Delete(It.IsAny<Guid>()), Times.Once);
            _mapperMock.Verify(m => m.Map<PostDTO>(post), Times.Once);
        }

        [Fact]
        public async Task GetById_WhenValidCommentsToLoad_ShouldReturnPostDTO()
        {
            var postId = Guid.NewGuid();
            var post = new Post { Id = postId, Title = "Test Title", Content = "Test Content", CreatedBy = "author1", Comments = new List<Comment> { new Comment { Id = Guid.NewGuid(), AuthorId = "author1", CommentText = "Comment 1" } } };
            var postDto = new PostDTO { Id = postId, Title = "Test Title", Content = "Test Content", CreatedBy = new UserShortcutDTO { UserId = "author1" }, Comments = new List<CommentDTO> { new CommentDTO { Id = post.Comments.First().Id, CommentText = "Comment 1", CreatedBy = new UserShortcutDTO { UserId = "author1" } } } };

            _repositoryMock.Setup(r => r.Get(postId, It.IsAny<PostWithCommentsSpecification>())).ReturnsAsync(post);
            _mapperMock.Setup(m => m.Map<PostDTO>(post)).Returns(postDto);
            _userSummaryServiceMock.Setup(u => u.GetUserShortcut("author1")).ReturnsAsync(new UserShortcutDTO { UserId = "author1", UserName = "Usernameauthor1" });

            var result = await _postService.GetById(postId,1);

            Assert.Equal(postDto.Id, result.Id);
            Assert.Equal(postDto.Title, result.Title);
            Assert.Equal(postDto.Content, result.Content);
            Assert.Equal(postDto.CreatedBy.UserId, result.CreatedBy.UserId);
            Assert.Equal(postDto.Comments.First().Id, result.Comments.First().Id);
            _repositoryMock.Verify(r => r.Get(It.IsAny<Guid>(), It.IsAny<PostWithCommentsSpecification>()), Times.Once);
            _mapperMock.Verify(m => m.Map<PostDTO>(post), Times.Once);
            _userSummaryServiceMock.Verify(u => u.GetUserShortcut(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task GetById_WhenInvalidCommentsToLoad_ShouldReturnNull()
        {
            var postId = Guid.NewGuid();

            var result = await _postService.GetById(postId, 0);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetPostShortcutsOfUser_WhenValidPaging_ShouldReturnPagedPostShortcuts()
        {
            var userId = "author1";
            var posts = new List<Post>
            {
                new Post { Id = Guid.NewGuid(), CreatedBy = userId, Title = "Post 1", Content = "Content 1" },
                new Post { Id = Guid.NewGuid(), CreatedBy = userId, Title = "Post 2", Content = "Content 2" }
            };
            var postShortcuts = new List<PostShortcutDTO>
            {
                new PostShortcutDTO { Id = posts[0].Id, Title = "Post 1" },
                new PostShortcutDTO { Id = posts[1].Id, Title = "Post 2" }
            };
            PostsOfUserSpecification capturedSpec = null;
            _repositoryMock
                .Setup(r => r.GetAll(It.IsAny<PostsOfUserSpecification>(), 1, 5))
                .ReturnsAsync(posts)
                .Callback<ISpecification<Post>, int, int>((spec, pageNumber, pageSize) =>
                {
                    capturedSpec = spec as PostsOfUserSpecification;
                });
            _mapperMock.Setup(m => m.Map<List<PostShortcutDTO>>(posts)).Returns(postShortcuts);

            var result = await _postService.GetPostShortcutsOfUser(userId, orderByDateAscending:true);

            Assert.Equal(postShortcuts.Count, result.Count);
            Assert.Equal(postShortcuts[0].Id, result[0].Id);
            Assert.Equal(postShortcuts[1].Id, result[1].Id);
            Assert.NotNull(capturedSpec);
            Assert.Equal(capturedSpec.OrderByAscending, true);
        }

        [Fact]
        public async Task GetPostShortcutsOfUser_WhenInvalidPaging_ShouldReturnNull()
        {
            var userId = "author1";

            var result = await _postService.GetPostShortcutsOfUser(userId, -1, -5);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetSummaries_WhenValidPaging_ShouldReturnPagedPostSummaries()
        {
            var posts = new List<Post>
            {
                new Post { Id = Guid.NewGuid(), CreatedBy = "author1", Title = "Post 1", Content = "Content 1" },
                new Post { Id = Guid.NewGuid(), CreatedBy = "author2", Title = "Post 2", Content = "Content 2" }
            };
            var postSummaries = new List<PostSummaryDTO>
            {
                new PostSummaryDTO { Id = posts[0].Id, Title = "Post 1", CreatedBy = new UserShortcutDTO { UserId = "author1" } },
                new PostSummaryDTO { Id = posts[1].Id, Title = "Post 2", CreatedBy = new UserShortcutDTO { UserId = "author2" } }
            };
            PostsOrderedByDateCreatedSpecification capturedSpec = null;

            _repositoryMock
                .Setup(r => r.GetAll(It.IsAny<BaseSpecification<Post>>(), 1, 5))
                .Callback<ISpecification<Post>, int, int>((spec, pageNumber, pageSize) =>
                {
                    capturedSpec = spec as PostsOrderedByDateCreatedSpecification;
                })
                .ReturnsAsync(posts);
            _mapperMock.Setup(m => m.Map<List<PostSummaryDTO>>(posts)).Returns(postSummaries);
            _userSummaryServiceMock
                .Setup(u => u.GetUserShortcut(It.IsAny<string>()))
                .ReturnsAsync((string userId) => new UserShortcutDTO { UserId = userId, UserName = "Username" + userId });

            var result = await _postService.GetSummaries(orderByDateAscending: true);

            Assert.NotNull(capturedSpec);
            Assert.True(capturedSpec.OrderByAscending);
            Assert.Equal(postSummaries.Count, result.Count);
            Assert.Equal(postSummaries[0].Id, result[0].Id);
            Assert.Equal(postSummaries[1].Id, result[1].Id);
            _repositoryMock.Verify(r => r.GetAll(It.IsAny<BaseSpecification<Post>>(), 1, 5), Times.Once);
            _mapperMock.Verify(m => m.Map<List<PostSummaryDTO>>(posts), Times.Once);
            _userSummaryServiceMock.Verify(u => u.GetUserShortcut(It.IsAny<string>()), Times.Exactly(postSummaries.Count));
        }

        [Fact]
        public async Task GetSummaries_WhenInvalidPaging_ShouldReturnNull()
        {
            var result = await _postService.GetSummaries(-1, -5);

            Assert.Null(result);
        }
        [Fact]
        public async Task GetSummaries_WhenPassingSearchTerm_ShouldCaptureTerm()
        {
            var posts = new List<Post>
            {
                new Post { Id = Guid.NewGuid(), CreatedBy = "author1", Title = "Post 1", Content = "Content 1" },
                new Post { Id = Guid.NewGuid(), CreatedBy = "author2", Title = "Post 2", Content = "Content 2" }
            };
            var postSummaries = new List<PostSummaryDTO>
            {
                new PostSummaryDTO { Id = posts[0].Id, Title = "Post 1", CreatedBy = new UserShortcutDTO { UserId = "author1" } },
                new PostSummaryDTO { Id = posts[1].Id, Title = "Post 2", CreatedBy = new UserShortcutDTO { UserId = "author2" } }
            };
            PostsWithSearchTermSpecification capturedSpec = null;
            _repositoryMock
                .Setup(r => r.GetAll(It.IsAny<PostsWithSearchTermSpecification>(), 1, 5))
                .Callback<ISpecification<Post>, int, int>((spec, pageNumber, pageSize) =>
                {
                    capturedSpec = spec as PostsWithSearchTermSpecification;
                })
                .ReturnsAsync(posts);
            _mapperMock.Setup(m => m.Map<List<PostSummaryDTO>>(posts)).Returns(postSummaries);
            _userSummaryServiceMock
                .Setup(u => u.GetUserShortcut(It.IsAny<string>()))
                .ReturnsAsync((string userId) => new UserShortcutDTO { UserId = userId, UserName = "Username" + userId });

            var result = await _postService.GetSummaries(searchTerm:"Post",orderByDateAscending: true);

            Assert.NotNull(capturedSpec);
            Assert.True(capturedSpec.OrderByAscending);
            Assert.Equal(postSummaries.Count, result.Count);
            Assert.Equal(postSummaries[0].Id, result[0].Id);
            Assert.Equal(postSummaries[1].Id, result[1].Id);
            _repositoryMock.Verify(r => r.GetAll(It.IsAny<BaseSpecification<Post>>(), 1, 5), Times.Once);
            _mapperMock.Verify(m => m.Map<List<PostSummaryDTO>>(posts), Times.Once);
            _userSummaryServiceMock.Verify(u => u.GetUserShortcut(It.IsAny<string>()), Times.Exactly(postSummaries.Count));
        }
        [Fact]
        public async Task Update_ShouldReturnUpdatedPostDTO()
        {
            var postId = Guid.NewGuid();
            var updatePostDto = new UpdatePostDTO { Title = "Updated Title", Content = "Updated Content" };
            var post = new Post { Id = postId, Title = "Original Title", Content = "Original Content", CreatedBy = "author1" };
            var updatedPost = new Post { Id = postId, Title = "Updated Title", Content = "Updated Content", CreatedBy = "author1" };
            var postDto = new PostDTO { Id = postId, Title = "Updated Title", Content = "Updated Content", CreatedBy = new UserShortcutDTO { UserId = "author1" } };

            _repositoryMock.Setup(r => r.Get(postId)).ReturnsAsync(post);
            _mapperMock.Setup(m => m.Map(updatePostDto, post)).Callback<UpdatePostDTO, Post>((dto, p) =>
            {
                p.Title = dto.Title;
                p.Content = dto.Content;
            });
            _repositoryMock.Setup(r => r.Update(post)).ReturnsAsync(updatedPost);
            _mapperMock.Setup(m => m.Map<PostDTO>(updatedPost)).Returns(postDto);

            var result = await _postService.Update(postId, updatePostDto);

            Assert.Equal(postDto.Id, result.Id);
            Assert.Equal(postDto.Title, result.Title);
            Assert.Equal(postDto.Content, result.Content);
            _repositoryMock.Verify(r => r.Get(It.IsAny<Guid>()), Times.Once);
            _repositoryMock.Verify(r => r.Update(It.IsAny<Post>()), Times.Once);
            _mapperMock.Verify(m => m.Map<UpdatePostDTO, Post>(updatePostDto, post), Times.Once);
            _mapperMock.Verify(m => m.Map<PostDTO>(updatedPost), Times.Once);
        }
    }
}
