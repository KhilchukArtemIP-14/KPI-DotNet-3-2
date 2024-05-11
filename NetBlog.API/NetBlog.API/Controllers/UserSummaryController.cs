using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBlog.BAL.Services.UserSummaryService;
using NetBlog.Common.DTO;

namespace NetBlog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserSummaryController : Controller
    {
        private readonly IUserSummaryService _userSummaryService;
        private readonly IAuthorizationService _authorizationService;

        public UserSummaryController(IUserSummaryService userSummaryService, IAuthorizationService authorizationService)
        {
            _userSummaryService = userSummaryService;
            _authorizationService = authorizationService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserSummary(string id)
        {
            var userSummary = await _userSummaryService.GetUserSummary(id);
            if (userSummary == null)
            {
                return NotFound("User not found");
            }
            return Ok(userSummary);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserSummary(string id, UpdateUserDTO dto)
        {
            var tryAuth = await _authorizationService.AuthorizeAsync(User, id, "CanUpdateUserSummary");
            if (tryAuth.Succeeded)
            {
                var updatedUserSummary = await _userSummaryService.UpdateUserSummaryById(dto, id);
                if (updatedUserSummary == null)
                {
                    return NotFound("User not found");
                }
                return Ok(updatedUserSummary);
            }

            return new ForbidResult();
        }
    }
}
