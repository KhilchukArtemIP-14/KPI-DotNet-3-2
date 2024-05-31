using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using NetBlog.BAL.Services.PostsServices;
using NetBlog.BAL.DTO;
using NetBlog.DAL.Repositories;

namespace NetBlog.API.Authorization
{
    public class CanModifyPostHandler : AuthorizationHandler<CanModifyPostRequirement, Guid>
    {
        private readonly IPostService _postService;
        public CanModifyPostHandler(IPostService postService)
        {
            _postService = postService;
        }
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            CanModifyPostRequirement requirement,
            Guid postId)
        {
            var entity = await _postService.GetById(postId);
            if (entity == null) context.Succeed(requirement); // no post no problems XD
            var userIdClaim = context.User.FindFirst("userId");
            if (context.User.IsInRole("Author")
                && userIdClaim != null
                && entity!= null
                && entity.CreatedBy.UserId == userIdClaim.Value)
            {
                context.Succeed(requirement);
                return;
            }
        }
    }
}
