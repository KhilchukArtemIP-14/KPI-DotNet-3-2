using MediatR;
using Microsoft.AspNetCore.Identity;
using NetBlog.Application.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Application.Features.Authorization.Commands
{
    public class RegisterCommand: IRequest<IdentityResult>
    {
        [Required]
        [MinLength(1)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        [Required]
        public string[] Roles { get; set; }
        public class RegisterCommandHandler : IRequestHandler<RegisterCommand, IdentityResult>
        {
            private readonly UserManager<IdentityUser> _userManager;

            public RegisterCommandHandler(UserManager<IdentityUser> userManager)
            {
                _userManager = userManager;
            }

            public async Task<IdentityResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
            {
                var identityUser = new IdentityUser()
                {
                    UserName = request.Name,
                    Email = request.Email,
                };

                var identityResult = await _userManager.CreateAsync(identityUser, request.Password);
                if (identityResult.Succeeded)
                {
                    if (request.Roles != null && request.Roles.Any())
                    {
                        identityResult = await _userManager.AddToRolesAsync(identityUser, request.Roles);
                        if (identityResult.Succeeded)
                        {
                            var claim = new Claim("bio", "Empty");
                            identityResult = await _userManager.AddClaimAsync(identityUser, claim);
                            return identityResult;
                        }
                    }
                }
                return null;
            }
        }
    }
}
