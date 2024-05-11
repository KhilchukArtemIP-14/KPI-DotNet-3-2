using Microsoft.AspNetCore.Authorization;
using NetBlog.BAL.Services.CommentsService;
using NetBlog.BAL.Services.PostsServices;
using NetBlog.DAL.Models;

namespace NetBlog.API.Authorization
{
    public class CanDeleteCommentHandler : AuthorizationHandler<CanDeleteCommentRequirement, Guid>
    {
        private readonly ICommentsService _commentService;
        public CanDeleteCommentHandler(ICommentsService commentService)
        {
            _commentService = commentService;
        }
        protected async override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            CanDeleteCommentRequirement requirement,
            Guid commentId)
        {
            var entity = await _commentService.GetCommentById(commentId);
            var userIdClaim = context.User.FindFirst("userId");
            if (context.User.IsInRole("Author")
                ||(userIdClaim != null && entity !=null && entity.CreatedBy.UserId == userIdClaim.Value))
            {
                context.Succeed(requirement);
                return;
            }
        }
    }
}
