using Microsoft.AspNetCore.Identity;
using NetBlog.BAL.Services.UserSummaryService;
using NetBlog.BAL.DTO;
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

        public async Task<UserShortcutDTO> GetUserShortcut(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            var name = user != null ? user.UserName : "NOT FOUND";

            return new UserShortcutDTO
            {
                UserId = id,
                UserName = name
            };
        }

        public async Task<UserSummaryDTO> GetUserSummary(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            

            if (user != null)
            {
                var bioClaim = (await _userManager.GetClaimsAsync(user))
                    .FirstOrDefault(c => c.Type == "bio");
                if (bioClaim != null)
                {
                    var summary = new UserSummaryDTO()
                    {
                        Id = id,
                        Name = user.UserName,
                        Email = user.Email,
                        Roles = (await _userManager.GetRolesAsync(user)).ToArray(),
                        Bio = bioClaim.Value
                    };
                    return summary;
                }            }

            return null;
        }

        public async Task<UserSummaryDTO> UpdateUserSummaryById(UpdateUserDTO dto, string userId)
        {
            if (dto.Name == null || dto.Bio == null) return null;
            IdentityUser user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                user.UserName = dto.Name;

                IdentityResult identityResult = await _userManager.UpdateAsync(user);

                var bioClaim = (await _userManager.GetClaimsAsync(user))
                    .FirstOrDefault(c => c.Type == "bio");
                if (bioClaim != null)
                {
                    var result = await _userManager.RemoveClaimAsync(user, bioClaim);
                }
                var claim = new Claim("bio", dto.Bio);
                var resultAddClaim = await _userManager.AddClaimAsync(user, claim);

                if (identityResult.Succeeded && resultAddClaim.Succeeded)
                {
                    var updatedSummary = new UserSummaryDTO()
                    {
                        Id = userId,
                        Name = user.UserName,
                        Email = user.Email,
                        Roles = (await _userManager.GetRolesAsync(user)).ToArray(),
                        Bio = dto.Bio,
                    };
                    return updatedSummary;
                };
            }
            return null;
        }

    }
}
