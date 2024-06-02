using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NetBlog.Application.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Application.Features.Authorization.Commands
{
    public class LoginCommand:IRequest<LoginResponseDTO>
    {
        public LoginUserDTO LoginUserDTO { get; set; }
        public LoginCommand(LoginUserDTO loginUserDTO)
        {
            LoginUserDTO = loginUserDTO;
        }
        public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDTO>
        {
            private readonly UserManager<IdentityUser> _userManager;
            private readonly IConfiguration _config;
            public LoginCommandHandler(UserManager<IdentityUser> userManager, IConfiguration config)
            {
                _userManager = userManager;
                _config = config;
            }
            public async Task<LoginResponseDTO> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                var dto = request.LoginUserDTO;
                var user = await _userManager.FindByEmailAsync(dto.Email);

                if (user != null)
                {
                    var passwordCheck = await _userManager.CheckPasswordAsync(user, dto.Password);
                    if (passwordCheck)
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        if (roles != null)
                        {
                            var response = new LoginResponseDTO()
                            {
                                Name = user.UserName,
                                Email = user.Email,
                                UserId = user.Id,
                                Roles = roles.ToArray(),
                                Token = await CreateJWTToken(user, roles.ToList())
                            };

                            return response;
                        }
                    }
                }
                return null;
            }
            public async Task<string> CreateJWTToken(IdentityUser user, List<string> roles)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("userId", user.Id),
                };
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    _config["Jwt:Issuer"],
                    _config["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: credentials
                    );
                return new JwtSecurityTokenHandler().WriteToken(token);
            }


        }

    }
}
