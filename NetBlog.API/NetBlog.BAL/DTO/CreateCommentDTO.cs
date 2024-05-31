using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.BAL.DTO
{
    public class CreateCommentDTO
    {
        public Guid PostId { get; set; }
        public string AuthorId { get; set; }
        public string CommentText { get; set; }
    }
}
