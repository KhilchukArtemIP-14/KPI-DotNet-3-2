using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using NetBlog.Domain.Entity;
using NetBlog.Domain.RepositoryContracts;

namespace NetBlog.Persistance.Authorization
{
    public class CanModifyPostHandler : AuthorizationHandler<CanModifyPostRequirement, Guid>
    {
        private readonly IRepository<Post> _repository;
        public CanModifyPostHandler(IRepository<Post> repository)
        {
            _repository = repository;
        }
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            CanModifyPostRequirement requirement,
            Guid postId)
        {
            var entity = await _repository.Get(postId);
            if (entity == null) context.Succeed(requirement);
            var userIdClaim = context.User.FindFirst("userId");
            if (context.User.IsInRole("Author")
                && userIdClaim != null
                && entity!= null
                && entity.CreatedBy == userIdClaim.Value)
            {
                context.Succeed(requirement);
                return;
            }
        }
    }
}
