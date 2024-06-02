using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Application.DTOs
{
    public class CreatePostDTO
    {
        [Required]
        [MinLength(1)]
        public string Title { get; set; }
        [Required]
        [MinLength(1)]
        public string ContentPreview { get; set; }
        [Required]
        [MinLength(1)]
        public string Content { get; set; }
        [Required]
        public string CreatedBy { get; set; }
    }
}
