using Microsoft.AspNetCore.Authorization;

namespace NetBlog.API.Authorization
{
    public class CanDeleteCommentRequirement:IAuthorizationRequirement
    {
    }
}
