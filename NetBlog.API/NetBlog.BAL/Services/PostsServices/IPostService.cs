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
        Task<List<PostSummaryDTO>> GetSummaries();
        Task<PostDTO> GetById(Guid id);
        Task<PostDTO> Add(CreatePostDTO dto);
        Task<PostDTO> Update(Guid id, UpdatePostDTO dto);
        Task<PostDTO> Delete(Guid id);
    }
}
