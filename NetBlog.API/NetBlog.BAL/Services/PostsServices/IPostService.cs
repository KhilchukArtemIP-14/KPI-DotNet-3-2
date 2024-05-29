using NetBlog.Common.DTO;
using NetBlog.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.BAL.Services.PostsServices
{
    public interface IPostService
    {
        Task<List<PostSummaryDTO>> GetSummaries(int pageNumber = 1, int pageSize = 5);
        Task<PostDTO> GetById(Guid id, int commentsToLoad = 5);
        Task<PostDTO> Add(CreatePostDTO dto);
        Task<PostDTO> Update(Guid id, UpdatePostDTO dto);
        Task<List<PostShortcutDTO>> GetPostShortcutsOfUser(string userId, int pageNumber = 1, int pageSize = 5);
        Task<PostDTO> Delete(Guid id);
    }
}
