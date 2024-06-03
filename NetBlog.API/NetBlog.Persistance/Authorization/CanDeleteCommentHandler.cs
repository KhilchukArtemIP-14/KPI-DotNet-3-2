using Microsoft.AspNetCore.Authorization;
using NetBlog.Domain.Entity;
using NetBlog.Domain.RepositoryContracts;
using NetBlog.Persistance.Repository.Implementations;


namespace NetBlog.Persistance.Authorization
{
    public class CanDeleteCommentHandler : AuthorizationHandler<CanDeleteCommentRequirement, Guid>
    {
        private readonly IRepository<Comment> _repository;
        public CanDeleteCommentHandler(IRepository<Comment> repository)
        {
            _repository = repository;
        }
        protected async override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            CanDeleteCommentRequirement requirement,
            Guid commentId)
        {
            var entity = await _repository.Get(commentId);
            var userIdClaim = context.User.FindFirst("userId");
            if (context.User.IsInRole("Author")
                ||(userIdClaim != null && entity !=null && entity.AuthorId == userIdClaim.Value))
            {
                context.Succeed(requirement);
                return;
            }
        }
    }
}
