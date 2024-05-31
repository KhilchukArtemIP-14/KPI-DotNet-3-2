using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.BAL.DTO
{
    public class CommentDTO
    {
        public Guid Id { get; set; }
        public UserShortcutDTO CreatedBy { get; set; }
        public string CommentText { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
