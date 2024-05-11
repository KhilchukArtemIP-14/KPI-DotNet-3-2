using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Common.DTO
{
    public class CreatePostDTO
    {
        public string Title { get; set; }
        public string ContentPreview { get; set; }
        public string Content { get; set; }
        public string CreatedBy { get; set; }
    }
}
