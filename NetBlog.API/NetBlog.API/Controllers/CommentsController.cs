using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBlog.BAL.Services.CommentsService;
using NetBlog.Common.DTO;

namespace NetBlog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : Controller
    {
        private readonly ICommentsService _commentService;
        private readonly IAuthorizationService _authorizationService;

        public CommentsController(ICommentsService commentService, IAuthorizationService authorizationService)
        {
            _commentService = commentService;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateComment(CreateCommentDTO dto)
        {
            var comment = await _commentService.CreateComment(dto);
            return Ok(comment);
        }

        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            var tryAuth = await _authorizationService.AuthorizeAsync(User, commentId, "CanDeleteCommentPolicy");
            if (tryAuth.Succeeded)
            {
                var comment = await _commentService.DeleteComment(commentId);

                if (comment == null) return NotFound("Comment not found");

                return Ok(comment);
            }
            return new ForbidResult();
        }

        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetCommentsForPost(Guid postId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        {
            var comments = await _commentService.GetCommentsForPost(postId,pageNumber,pageSize);
            return Ok(comments);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetShortcutsForUser(string userId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        {
            var comments = await _commentService.GetCommentShortuctsOfUser(userId, pageNumber, pageSize);
            return Ok(comments);
        }
    }
}
