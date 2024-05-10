﻿using Microsoft.AspNetCore.Mvc;
using NetBlog.BAL.Services.AuthServices;
using NetBlog.Common.DTO;
using System.Net.NetworkInformation;

namespace NetBlog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDTO dto)
        {
            var result = await _authService.Register(dto);

            if (result!=null) return Ok("User successfully created");

            return BadRequest("Couldn't register user");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDTO dto)
        {
            var result = await _authService.Login(dto);

            if (result != null) return Ok(result);

            return BadRequest("Couldn't login user");
        }
    }
}
