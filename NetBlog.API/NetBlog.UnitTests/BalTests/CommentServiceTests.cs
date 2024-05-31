using AutoMapper;
using Moq;
using NetBlog.BAL.Services.CommentsService;
using NetBlog.BAL.Services.UserSummaryService;
using NetBlog.Common.DTO;
using NetBlog.DAL.Models;
using NetBlog.DAL.Repositories;
using NetBlog.DAL.Specifications.Implementations;
using NetBlog.DAL.Specifications.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NetBlog.UnitTests.BalTests
{
    public class CommentServiceTests
    {
        private readonly Mock<IRepository<Comment>> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserSummaryService> _userSummaryServiceMock;
        private readonly CommentService _commentService;

        public CommentServiceTests()
        {
            _repositoryMock = new Mock<IRepository<Comment>>();
            _mapperMock = new Mock<IMapper>();
            _userSummaryServiceMock = new Mock<IUserSummaryService>();
            _commentService = new CommentService(_repositoryMock.Object, _mapperMock.Object, _userSummaryServiceMock.Object);
        }
        [Fact]
        public async Task CreateComment_ShouldReturnCommentDTO()
        {
            var createCommentDto = new CreateCommentDTO { AuthorId = Guid.NewGuid().ToString(), CommentText = "Test comment", PostId = Guid.NewGuid() };
            var comment = new Comment { Id = Guid.NewGuid(), AuthorId = createCommentDto.AuthorId, CommentText = "Test comment", PostId = createCommentDto.PostId };
            var commentDto = new CommentDTO { Id = comment.Id, CreatedBy = new UserShortcutDTO { UserId = createCommentDto.AuthorId }, CommentText = "Test comment" };

            _mapperMock.Setup(m => m.Map<Comment>(createCommentDto)).Returns(comment);
            _repositoryMock.Setup(r => r.Add(comment)).ReturnsAsync(comment);
            _mapperMock.Setup(m => m.Map<CommentDTO>(comment)).Returns(commentDto);
            _userSummaryServiceMock.Setup(u => u.GetUserShortcut(createCommentDto.AuthorId)).ReturnsAsync(new UserShortcutDTO { UserId = createCommentDto.AuthorId });

            var result = await _commentService.CreateComment(createCommentDto);

            Assert.Equal(commentDto.Id, result.Id);
            Assert.Equal(commentDto.CommentText, result.CommentText);
            Assert.Equal(commentDto.CreatedBy.UserId, result.CreatedBy.UserId);
            _repositoryMock.Verify(r => r.Add(It.IsAny<Comment>()), Times.Once);
            _mapperMock.Verify(m => m.Map<CommentDTO>(comment), Times.Once);
        }

        [Fact]
        public async Task DeleteComment_ShouldDeleteAndReturnCommentDTO()
        {
            var commentId = Guid.NewGuid();
            var comment = new Comment { Id = commentId, AuthorId = "author1", CommentText = "Test comment", IsDeleted = false };
            var commentDto = new CommentDTO { Id = commentId, CreatedBy = new UserShortcutDTO { UserId = "author1" }, CommentText = "Test comment" };

            _repositoryMock.Setup(r => r.Delete(commentId)).ReturnsAsync(comment);
            _mapperMock.Setup(m => m.Map<CommentDTO>(comment)).Returns(commentDto);

            var result = await _commentService.DeleteComment(commentId);

            Assert.Equal(commentDto.Id, result.Id);
            Assert.Equal(commentDto.CommentText, result.CommentText);
            _repositoryMock.Verify(r => r.Delete(It.IsAny<Guid>()),Times.Once);
            _mapperMock.Verify(m => m.Map<CommentDTO>(comment), Times.Once);
        }

        [Fact]
        public async Task GetCommentById_ShouldReturnCommentDTO()
        {
            var commentId = Guid.NewGuid();
            var comment = new Comment { Id = commentId, AuthorId = "author1", CommentText = "Test comment" };
            var commentDto = new CommentDTO { Id = commentId, CreatedBy = new UserShortcutDTO { UserId = "author1" }, CommentText = "Test comment" };

            _repositoryMock.Setup(r => r.Get(commentId)).ReturnsAsync(comment);
            _mapperMock.Setup(m => m.Map<CommentDTO>(comment)).Returns(commentDto);

            var result = await _commentService.GetCommentById(commentId);

            Assert.Equal(commentDto.Id, result.Id);
            Assert.Equal(commentDto.CommentText, result.CommentText);
            _repositoryMock.Verify(r => r.Get(It.IsAny<Guid>()), Times.Once);
            _mapperMock.Verify(m => m.Map<CommentDTO>(comment), Times.Once);
        }

        [Fact]
        public async Task GetCommentsForPost_WhenValidPaging_ShouldReturnPagedComments()
        {
            var postId = Guid.NewGuid();
            var comments = new List<Comment>
                {
                    new Comment { Id = Guid.NewGuid(), AuthorId = "author1", CommentText = "Comment 1", PostId = postId, IsDeleted = false },
                    new Comment { Id = Guid.NewGuid(), AuthorId = "author2", CommentText = "Comment 2", PostId = postId, IsDeleted = false }
                };
            var commentDtos = new List<CommentDTO>
                {
                    new CommentDTO { Id = comments[0].Id, CreatedBy = new UserShortcutDTO { UserId = "author1" }, CommentText = "Comment 1" },
                    new CommentDTO { Id = comments[1].Id, CreatedBy = new UserShortcutDTO { UserId = "author2" }, CommentText = "Comment 2" }
                };
            CommentsForPostSpecification capturedSpec = null;

            _repositoryMock.Setup(r => r.GetAll(It.IsAny<CommentsForPostSpecification>(), 1, 5))
                .ReturnsAsync(comments)
                .Callback<ISpecification<Comment>, int, int>((spec, pageNumber, pageSize) =>
            {
                capturedSpec = spec as CommentsForPostSpecification;
            });
            _mapperMock.Setup(m => m.Map<List<CommentDTO>>(comments)).Returns(commentDtos);
            _userSummaryServiceMock
                .Setup(u => u.GetUserShortcut(It.IsAny<string>()))
                .ReturnsAsync((string userId) => new UserShortcutDTO { UserId = userId, UserName = "Username"+userId });

            var result = await _commentService.GetCommentsForPost(postId,orderByDateAscending:true);

            Assert.NotNull(capturedSpec);
            Assert.NotNull(capturedSpec.Criteria);
            Assert.True(capturedSpec.OrderByAscending); 
            Assert.Equal(commentDtos.Count, result.Count);
            Assert.Equal(commentDtos[0].Id, result[0].Id);
            Assert.Equal(commentDtos[1].Id, result[1].Id);
            Assert.Equal(commentDtos[1].Id, result[1].Id);
            _repositoryMock.Verify(r => r.GetAll(It.IsAny<CommentsForPostSpecification>(), 1, 5), Times.Once);
            _mapperMock.Verify(m => m.Map<List<CommentDTO>>(comments), Times.Once);
            _userSummaryServiceMock.Verify(u => u.GetUserShortcut(It.IsAny<string>()), Times.Exactly(commentDtos.Count));
        }
        [Fact]
        public async Task GetCommentsForPost_WhenInvalidPaging_ShouldReturnNull()
        {
            var postId = Guid.NewGuid();

            var result = await _commentService.GetCommentsForPost(postId, -1,-5,orderByDateAscending: true);

            Assert.Null(result);
        }
        [Fact]
        public async Task GetCommentShortcutsOfUser_ShouldReturnPagedCommentShortcuts()
        {
            var userId = "author1";
            var comments = new List<Comment>
                {
                    new Comment { Id = Guid.NewGuid(), AuthorId = userId, CommentText = "Comment 1", Post = new Post { Id = Guid.NewGuid(), Title = "Post Title 1" }, IsDeleted = false },
                    new Comment { Id = Guid.NewGuid(), AuthorId = userId, CommentText = "Comment 2", Post = new Post { Id = Guid.NewGuid(), Title = "Post Title 2" }, IsDeleted = false }
                };
            var commentShortcuts = new List<CommentShortcutDTO>
                {
                    new CommentShortcutDTO { PostId = comments[0].Post.Id, PostTitle = comments[0].Post.Title, CommentText = "Comment 1" },
                    new CommentShortcutDTO { PostId = comments[1].Post.Id, PostTitle = comments[1].Post.Title, CommentText = "Comment 2" }
                };
            CommentsOfUserSpecification capturedSpec = null;
            _repositoryMock
                .Setup(r => r.GetAll(It.IsAny<CommentsOfUserSpecification>(), 1, 5))
                .ReturnsAsync(comments)
                .Callback<ISpecification<Comment>, int, int>((spec, pageNumber, pageSize) =>
                {
                    capturedSpec = spec as CommentsOfUserSpecification;
                }); ;
            _mapperMock.Setup(m => m.Map<List<CommentShortcutDTO>>(comments)).Returns(commentShortcuts);
            _userSummaryServiceMock.Setup(u => u.GetUserShortcut(It.IsAny<string>())).ReturnsAsync((string userId) => new UserShortcutDTO { UserId = userId });

            var result = await _commentService.GetCommentShortuctsOfUser(userId);

            Assert.Equal(commentShortcuts.Count, result.Count);
            Assert.Equal(commentShortcuts[0].PostId, result[0].PostId);
            Assert.Equal(commentShortcuts[1].PostId, result[1].PostId);
        }

    }
}
