using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.BAL.DTO
{
    public class CommentShortcutDTO
    {
        public Guid PostId { get; set; }
        public string PostTitle { get; set; }
        public string CommentText { get; set; }
    }
}
