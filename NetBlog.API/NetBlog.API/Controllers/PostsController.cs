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

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(CreatePostDTO dto)
        {
            var post = await _postService.Add(dto);
            return Ok(post);
        }

        //[Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var post = await _postService.Delete(id);

            if(post == null) return NotFound("Post not found");

            return Ok(post);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var post = await _postService.GetById(id);

            if (post == null) return NotFound("Post not found");

            return Ok(post);
        }

        [HttpGet]
        public async Task<IActionResult> GetSummaries()
        {
            var postSummaries = await _postService.GetSummaries();
            return Ok(postSummaries);
        }

        //[Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdatePostDTO dto)
        {
            var post = await _postService.Update(id, dto);

            if (post == null) return NotFound("Post not found");

            return Ok(post);
        }
    }
}
