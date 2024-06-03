using AutoMapper;
using NetBlog.BAL.DTO;
using NetBlog.BAL.Mappings;
using NetBlog.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.UnitTests.BalTests
{
    public class AutomapperTests
    {
        private readonly IMapper _mapper;

        public AutomapperTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            _mapper = config.CreateMapper();
        }
        [Fact]
        public void CommentToCommentDTO_MapsCorrectly()
        {
            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                AuthorId = "author1",
                CommentText = "comment",
                DateCreated = DateTime.Now
            };

            var result = _mapper.Map<CommentDTO>(comment);

            Assert.Equal(comment.Id, result.Id);
            Assert.Equal(comment.CommentText, result.CommentText);
            Assert.Equal(comment.DateCreated, result.DateCreated);
            Assert.Equal(comment.AuthorId, result.CreatedBy.UserId);
        }
        [Fact]
        public void CommentToCommentShortcutDTO_MapsCorrectly()
        {
            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                Post = new Post { Id = Guid.NewGuid(), Title = "title" },
                CommentText = "comment"
            };

            var result = _mapper.Map<CommentShortcutDTO>(comment);

            Assert.Equal(comment.Post.Id, result.PostId);
            Assert.Equal(comment.Post.Title, result.PostTitle);
            Assert.Equal(comment.CommentText, result.CommentText);
        }
        [Fact]
        public void PostToPostDTO_MapsCorrectly()
        {
            var post = new Post
            {
                Id = Guid.NewGuid(),
                Title = "title",
                ContentPreview = "preview",
                Content = "content",
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                CreatedBy = "author1",
                Comments = new List<Comment>
        {
            new Comment { Id = Guid.NewGuid(), IsDeleted = false },
            new Comment { Id = Guid.NewGuid(), IsDeleted = true }
        }
            };

            var result = _mapper.Map<PostDTO>(post);

            Assert.Equal(post.Id, result.Id);
            Assert.Equal(post.Title, result.Title);
            Assert.Equal(post.ContentPreview, result.ContentPreview);
            Assert.Equal(post.Content, result.Content);
            Assert.Equal(post.DateCreated, result.DateCreated);
            Assert.Equal(post.DateUpdated, result.DateUpdated);
            Assert.Equal(post.CreatedBy, result.CreatedBy.UserId);
            Assert.Single(result.Comments);
            Assert.Equal(post.Comments.First().Id, result.Comments.First().Id);
        }

        [Fact]
        public void PostToPostSummaryDTO_MapsCorrectly()
        {
            var post = new Post
            {
                Id = Guid.NewGuid(),
                Title = "title",
                ContentPreview = "preview",
                DateCreated = DateTime.Now,
                CreatedBy = "author1"
            };

            var result = _mapper.Map<PostSummaryDTO>(post);

            Assert.Equal(post.Id, result.Id);
            Assert.Equal(post.Title, result.Title);
            Assert.Equal(post.ContentPreview, result.ContentPreview);
            Assert.Equal(post.DateCreated, result.DateCreated);
            Assert.Equal(post.CreatedBy, result.CreatedBy.UserId);
        }

        [Fact]
        public void PostToPostShortcutDTO_MapsCorrectly()
        {
            var post = new Post
            {
                Id = Guid.NewGuid(),
                Title = "title",
                DateCreated = DateTime.Now
            };

            var result = _mapper.Map<PostShortcutDTO>(post);

            Assert.Equal(post.Id, result.Id);
            Assert.Equal(post.Title, result.Title);
            Assert.Equal(post.DateCreated, result.DateCreated);
        }

        [Fact]
        public void CreateCommentDTOToComment_MapsCorrectly()
        {
            var createCommentDto = new CreateCommentDTO
            {
                PostId = Guid.NewGuid(),
                AuthorId = "author1",
                CommentText = "comment"
            };

            var result = _mapper.Map<Comment>(createCommentDto);

            Assert.Equal(createCommentDto.PostId, result.PostId);
            Assert.Equal(createCommentDto.AuthorId, result.AuthorId);
            Assert.Equal(createCommentDto.CommentText, result.CommentText);
            Assert.Equal(DateTime.Now.Date, result.DateCreated.Date);
        }

        [Fact]
        public void CreatePostDTOToPost_MapsCorrectly()
        {
            var createPostDto = new CreatePostDTO
            {
                Title = "title",
                ContentPreview = "preview",
                Content = "content",
                CreatedBy = "author1"
            };

            var result = _mapper.Map<Post>(createPostDto);

            Assert.Equal(createPostDto.Title, result.Title);
            Assert.Equal(createPostDto.ContentPreview, result.ContentPreview);
            Assert.Equal(createPostDto.Content, result.Content);
            Assert.Equal(createPostDto.CreatedBy, result.CreatedBy);
            Assert.Equal(DateTime.Now.Date, result.DateCreated.Date);
        }

        [Fact]
        public void UpdatePostDTOToPost_MapsCorrectly()
        {
            var updatePostDto = new UpdatePostDTO
            {
                Title = "title1",
                ContentPreview = "preview1",
                Content = "content1"
            };

            var result = _mapper.Map<Post>(updatePostDto);

            Assert.Equal(updatePostDto.Title, result.Title);
            Assert.Equal(updatePostDto.ContentPreview, result.ContentPreview);
            Assert.Equal(updatePostDto.Content, result.Content);
            Assert.Equal(DateTime.Now.Date, result.DateUpdated?.Date);
        }



    }
}
