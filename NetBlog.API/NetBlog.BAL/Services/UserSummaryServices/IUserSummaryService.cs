using NetBlog.BAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.BAL.Services.UserSummaryService
{
    public interface IUserSummaryService
    {
        public Task<UserSummaryDTO> GetUserSummary(string id);
        public Task<UserShortcutDTO> GetUserShortcut(string id);
        public Task<UserSummaryDTO> UpdateUserSummaryById(UpdateUserDTO dto, string userId);
    }
}
