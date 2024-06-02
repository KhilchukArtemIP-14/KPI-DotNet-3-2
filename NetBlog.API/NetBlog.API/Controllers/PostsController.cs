using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBlog.Application.DTOs;
using NetBlog.Application.Features.Posts.Commands;
using NetBlog.Application.Features.Posts.Queries;

namespace NetBlog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IAuthorizationService _authorizationService;

        public PostsController(IMediator mediator, IAuthorizationService authorizationService)
        {
            _mediator = mediator;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        //[Authorize(Roles = "Author")]
        public async Task<IActionResult> Add(CreatePostDTO dto)
        {
            var post = await _mediator.Send(new CreatePostCommand(dto));
            return Ok(post);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            //var tryAuth = await _authorizationService.AuthorizeAsync(User, id, "CanModifyPostPolicy");

           //if (tryAuth.Succeeded)
            //{
                var post = await _mediator.Send(new DeletePostCommand(id));

                if (post == null) return NotFound("Post not found");

                return Ok(post);
            //}

            //return new ForbidResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, [FromQuery] int commentsToLoad = 5)
        {
            var post = await _mediator.Send(
                new GetPostByIdQuery(id,commentsToLoad));

            if (post == null) return NotFound("Post not found");

            return Ok(post);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPostShortcutsOfUser(string userId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] bool orderByDateAscending = false)
        {
            var post = await _mediator.Send(
                new GetPostShortcutsOfUserQuery(userId, pageNumber, pageSize, orderByDateAscending));

            if (post == null) return NotFound("Post not found");

            return Ok(post);
        }

        [HttpGet]
        public async Task<IActionResult> GetPostSummaries([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] string? searchQuery = null, [FromQuery] bool orderByDateAscending = false)
        {
            var postSummaries = await _mediator.Send(
                new GetPostSummariesQuery(pageNumber, pageSize, searchQuery, orderByDateAscending));

            return Ok(postSummaries);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdatePostDTO dto)
        {
            //var tryAuth = await _authorizationService.AuthorizeAsync(User, id, "CanModifyPostPolicy");

            //if (tryAuth.Succeeded)
            //{
                var post = await _mediator.Send(new UpdatePostCommand(id,dto));

                if (post == null) return NotFound("Post not found");

                return Ok(post);
            //}
            //return new ForbidResult();
        }
    }
}
