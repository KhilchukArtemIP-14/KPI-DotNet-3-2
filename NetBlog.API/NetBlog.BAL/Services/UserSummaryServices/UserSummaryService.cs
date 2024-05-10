using Microsoft.AspNetCore.Identity;
using NetBlog.BAL.Services.UserSummaryService;
using NetBlog.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.BAL.Services.UserSummaryServices
{
    public class UserSummaryService:IUserSummaryService
    {
        private readonly UserManager<IdentityUser> _userManager;
        public UserSummaryService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserSummaryDTO> GetUserSummary(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                var summary = new UserSummaryDTO()
                {
                    Id = id,
                    Name = user.UserName,
                    Email = user.Email,
                };
                return summary;
            }

            return null;
        }

        public async Task<UserSummaryDTO> UpdateUserSummaryById(UpdateUserDTO dto, string userId)
        {
            IdentityUser user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                user.UserName = dto.Name;

                IdentityResult identityResult = await _userManager.UpdateAsync(user);
                if (identityResult.Succeeded)
                {
                    var updatedSummary = new UserSummaryDTO()
                    {
                        Name = user.UserName,
                        Email = user.Email,
                    };
                    return updatedSummary;
                };
            }
            return null;
        }
    }
}
