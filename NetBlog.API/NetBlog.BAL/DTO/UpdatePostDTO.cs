using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.BAL.DTO
{
    public class UpdatePostDTO
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
    }
}
