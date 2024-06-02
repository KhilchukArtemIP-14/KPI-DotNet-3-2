using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using NetBlog.Application.DTOs;
using NetBlog.Application.Features.Authorization.Commands;

namespace NetBlog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDTO dto)
        {
            var result = await _mediator.Send(new RegisterCommand(dto));

            if (result == null) return BadRequest(new { Message = "Couldn't register user" });

            return Ok(new { Message = "User successfully created" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDTO dto)
        {
            var result = await _mediator.Send(new LoginCommand(dto));

            if (result == null) return BadRequest("Couldn't login user");

            return Ok(result);
        }
    }
}
