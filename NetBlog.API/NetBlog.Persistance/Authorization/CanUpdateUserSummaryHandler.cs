using Microsoft.AspNetCore.Authorization;

namespace NetBlog.Persistance.Authorization
{
    public class CanUpdateUserSummaryHandler : AuthorizationHandler<CanUpdateUserSummaryRequirement, string>
    {
        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanUpdateUserSummaryRequirement requirement, string userId)
        {
            var userIdClaim = context.User.FindFirst("userId");
            if (userIdClaim!=null&& userIdClaim.Value==userId)
            {
                context.Succeed(requirement);
                return;
            }
        }
    }
}
