using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBlog.BAL.Services.PostsServices;
using NetBlog.Common.DTO;

namespace NetBlog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        private readonly IAuthorizationService _authorizationService;

        public PostsController(IPostService postService, IAuthorizationService authorizationService)
        {
            _postService = postService;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [Authorize(Roles="Author")]
        public async Task<IActionResult> Add(CreatePostDTO dto)
        {
            var post = await _postService.Add(dto);
            return Ok(post);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var tryAuth = await _authorizationService.AuthorizeAsync(User, id, "CanModifyPostPolicy");

            if (tryAuth.Succeeded) 
            {
                var post = await _postService.Delete(id);

                if (post == null) return NotFound("Post not found");

                return Ok(post);
            }

            return new ForbidResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, [FromQuery] int commentsToLoad = 5)
        {
            var post = await _postService.GetById(id);

            if (post == null) return NotFound("Post not found");

            return Ok(post);
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPostShortcutsOfUser(string userId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        {
            var post = await _postService.GetPostShortcutsOfUser(userId, pageNumber, pageSize);

            if (post == null) return NotFound("Post not found");

            return Ok(post);
        }
        [HttpGet]
        public async Task<IActionResult> GetPostSummaries([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        {
            var postSummaries = await _postService.GetSummaries(pageNumber,pageSize);
            return Ok(postSummaries);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdatePostDTO dto)
        {
            var tryAuth = await _authorizationService.AuthorizeAsync(User, id, "CanModifyPostPolicy");

            if(tryAuth.Succeeded)
            {
                var post = await _postService.Update(id, dto);

                if (post == null) return NotFound("Post not found");

                return Ok(post);
            }
            return new ForbidResult();
        }
    }
}
