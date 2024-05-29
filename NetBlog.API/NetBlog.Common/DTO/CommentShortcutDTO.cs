using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Common.DTO
{
    public class CommentShortcutDTO
    {
        public Guid PostId { get; set; }
        public string PostTitle { get; set; }
        public string CommentText { get; set; }
    }
}
