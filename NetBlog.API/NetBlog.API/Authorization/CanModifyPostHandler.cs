using Microsoft.AspNetCore.Authorization;
using NetBlog.Common.DTO;

namespace NetBlog.API.Authorization
{
    public class CanModifyPostHandler : AuthorizationHandler<CanModifyPostRequirement, PostDTO>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanModifyPostRequirement requirement, PostDTO resource)
        {
            throw new NotImplementedException();
        }
    }
}
