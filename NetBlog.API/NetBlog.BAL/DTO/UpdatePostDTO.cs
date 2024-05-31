using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.BAL.DTO
{
    public class UpdatePostDTO
    {
        public string Title { get; set; }
        public string ContentPreview { get; set; }
        public string Content { get; set; }
    }
}
