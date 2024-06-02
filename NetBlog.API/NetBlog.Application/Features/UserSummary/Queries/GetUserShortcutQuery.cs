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
    public class GetUserShortcutQuery : IRequest<UserShortcutDTO>
    {
        public string UserId { get; set; }

        public GetUserShortcutQuery(string userId)
        {
            UserId = userId;
        }
        public class GetUserShortcutQueryHandler : IRequestHandler<GetUserShortcutQuery, UserShortcutDTO>
        {
            private readonly UserManager<IdentityUser> _userManager;

            public GetUserShortcutQueryHandler(UserManager<IdentityUser> userManager)
            {
                _userManager = userManager;
            }

            public async Task<UserShortcutDTO> Handle(GetUserShortcutQuery request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByIdAsync(request.UserId);

                var name = user != null ? user.UserName : "NOT FOUND";

                return new UserShortcutDTO
                {
                    UserId = request.UserId,
                    UserName = name
                };
            }
        }
    }
}
