using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBlog.Application.DTOs;
using NetBlog.Application.Features.UserSummary.Commands;
using NetBlog.Application.Features.UserSummary.Queries;

namespace NetBlog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserSummaryController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IAuthorizationService _authorizationService;

        public UserSummaryController(IMediator mediator, IAuthorizationService authorizationService)
        {
            _mediator = mediator;
            _authorizationService = authorizationService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserSummary(string id)
        {
            var userSummary = await _mediator.Send(new GetUserSummaryQuery(id));
            if (userSummary == null)
            {
                return NotFound("User not found");
            }
            return Ok(userSummary);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserSummary(UpdateUserSummaryCommand command)
        {
            //to-do: add auth
            var updatedUserSummary = await _mediator.Send(command);
            if (updatedUserSummary == null)
            {
                return NotFound("User not found");
            }
            return Ok(updatedUserSummary);
        }
    }
}
