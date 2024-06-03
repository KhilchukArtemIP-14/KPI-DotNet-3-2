using NetBlog.BAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.BAL.Services.CommentsService
{
    public interface ICommentsService
    {
        public Task<List<CommentDTO>> GetCommentsForPost(Guid postId, int pageNumber = 1, int pageSize = 5, bool orderByDateAscending = false);
        public Task<CommentDTO> CreateComment(CreateCommentDTO dto);
        public Task<CommentDTO> GetCommentById(Guid id);
        public Task<List<CommentShortcutDTO>> GetCommentShortuctsOfUser(string userId, int pageNumber = 1, int pageSize = 5, bool orderByDateAscending = false);

        public Task<CommentDTO> DeleteComment(Guid commentId);
    }
}
