using NetBlog.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.BAL.Services.CommentsService
{
    public interface ICommentsService
    {
        public Task<List<CommentDTO>> GetCommentsForPost(Guid postId);
        public Task<CommentDTO> CreateComment(CreateCommentDTO dto);
        public Task<CommentDTO> GetCommentById(Guid id);
        public Task<CommentDTO> DeleteComment(Guid commentId);
    }
}
