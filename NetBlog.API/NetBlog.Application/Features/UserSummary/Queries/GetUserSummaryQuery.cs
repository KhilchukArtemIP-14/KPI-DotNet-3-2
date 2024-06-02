using MediatR;
using Microsoft.AspNetCore.Identity;
using NetBlog.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Application.Features.UserSummary.Queries
{
    public class GetUserSummaryQuery : IRequest<UserSummaryDTO>
    {
        public string UserId { get; set; }

        public GetUserSummaryQuery(string userId)
        {
            UserId = userId;
        }
        public class GetUserSummaryQueryHandler : IRequestHandler<GetUserSummaryQuery, UserSummaryDTO>
        {
            private readonly UserManager<IdentityUser> _userManager;

            public GetUserSummaryQueryHandler(UserManager<IdentityUser> userManager)
            {
                _userManager = userManager;
            }

            public async Task<UserSummaryDTO> Handle(GetUserSummaryQuery request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByIdAsync(request.UserId);

                if (user != null)
                {
                    var bioClaim = (await _userManager.GetClaimsAsync(user))
                        .FirstOrDefault(c => c.Type == "bio");
                    if (bioClaim != null)
                    {
                        var summary = new UserSummaryDTO()
                        {
                            Id = request.UserId,
                            Name = user.UserName,
                            Email = user.Email,
                            Roles = (await _userManager.GetRolesAsync(user)).ToArray(),
                            Bio = bioClaim.Value
                        };
                        return summary;
                    }
                }
                return null;
            }
        }

    }
}
