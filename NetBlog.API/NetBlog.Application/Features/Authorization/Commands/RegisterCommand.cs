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
        public RegisterUserDTO RegisterUserDTO { get; set; }

        public RegisterCommand(RegisterUserDTO registerUserDTO)
        {
            RegisterUserDTO = registerUserDTO;
        }

        public class RegisterCommandHandler : IRequestHandler<RegisterCommand, IdentityResult>
        {
            private readonly UserManager<IdentityUser> _userManager;

            public RegisterCommandHandler(UserManager<IdentityUser> userManager)
            {
                _userManager = userManager;
            }

            public async Task<IdentityResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
            {
                var dto = request.RegisterUserDTO;
                var identityUser = new IdentityUser()
                {
                    UserName = dto.Name,
                    Email = dto.Email,
                };

                var identityResult = await _userManager.CreateAsync(identityUser, dto.Password);
                if (identityResult.Succeeded)
                {
                    if (dto.Roles != null && dto.Roles.Any())
                    {
                        identityResult = await _userManager.AddToRolesAsync(identityUser, dto.Roles);
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
