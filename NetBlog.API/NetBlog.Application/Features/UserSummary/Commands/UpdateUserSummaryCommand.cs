﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetBlog.Application.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Application.Features.UserSummary.Commands
{
    public class UpdateUserSummaryCommand : IRequest<UserSummaryDTO>
    {
        [Required]
        [MinLength(1)]
        [FromRoute(Name = "id")]
        public string UserId { get; set; }
        [Required]
        [MinLength(1)]
        public string Name { get; set; }
        [Required]
        [MinLength(1)]
        public string Bio { get; set; }

        public class UpdateUserSummaryCommandHandler : IRequestHandler<UpdateUserSummaryCommand, UserSummaryDTO>
        {
            private readonly UserManager<IdentityUser> _userManager;

            public UpdateUserSummaryCommandHandler(UserManager<IdentityUser> userManager)
            {
                _userManager = userManager;
            }
            public async Task<UserSummaryDTO> Handle(UpdateUserSummaryCommand request, CancellationToken cancellationToken)
            {
                if (request.Name == null || request.Bio == null) return null;

                var user = await _userManager.FindByIdAsync(request.UserId);

                if (user != null)
                {
                    user.UserName = request.Name;
                    var identityResult = await _userManager.UpdateAsync(user);

                    var bioClaim = (await _userManager.GetClaimsAsync(user))
                        .FirstOrDefault(c => c.Type == "bio");

                    if (bioClaim != null)
                    {
                        await _userManager.RemoveClaimAsync(user, bioClaim);
                    }

                    var claim = new Claim("bio", request.Bio);
                    var resultAddClaim = await _userManager.AddClaimAsync(user, claim);

                    if (identityResult.Succeeded && resultAddClaim.Succeeded)
                    {
                        var updatedSummary = new UserSummaryDTO()
                        {
                            Id = request.UserId,
                            Name = user.UserName,
                            Email = user.Email,
                            Roles = (await _userManager.GetRolesAsync(user)).ToArray(),
                            Bio = request.Bio
                        };
                        return updatedSummary;
                    }
                }
                return null;
            }
        }
    }
}
