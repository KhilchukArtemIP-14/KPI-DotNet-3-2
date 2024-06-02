using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Application.DTOs
{
    public class CreateCommentDTO
    {
        [Required]
        public Guid PostId { get; set; }
        [Required]
        public string AuthorId { get; set; }
        [Required]
        [MinLength(1)]
        public string CommentText { get; set; }
    }
}
