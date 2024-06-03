using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBlog.Application.DTOs;
using NetBlog.Application.Features.Comments.Commands;
using NetBlog.Application.Features.Comments.Queries;

namespace NetBlog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IAuthorizationService _authorizationService;

        public CommentsController(IMediator mediator, IAuthorizationService authorizationService)
        {
            _mediator = mediator;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateComment(CreateCommentDTO dto)
        {
            var comment = await _mediator.Send(new CreateCommentCommand(dto));

            return Ok(comment);
        }

        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            var tryAuth = await _authorizationService.AuthorizeAsync(User, commentId, "CanDeleteCommentPolicy");
            if (tryAuth.Succeeded)
            {
                var comment = await _mediator.Send(new DeleteCommentCommand(commentId));

                if (comment == null) return NotFound("Comment not found");

                return Ok(comment);
            }
            return new ForbidResult();
        }

        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetCommentsForPost(Guid postId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] bool orderByDateAscending = false)
        {
            var comments = await _mediator.Send(new GetCommentsForPostQuery(postId, pageNumber, pageSize, orderByDateAscending));

            return Ok(comments);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetShortcutsForUser(string userId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] bool orderByDateAscending = false)
        {
            var comments = await _mediator.Send(new GetCommentShortcutsOfUserQuery(userId, pageNumber, pageSize, orderByDateAscending));

            return Ok(comments);
        }
    }

}
