using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Application.DTOs
{
    public class PostDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ContentPreview { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public UserShortcutDTO CreatedBy { get; set; }
        public IEnumerable<CommentDTO> Comments { get; set; }
    }
}
