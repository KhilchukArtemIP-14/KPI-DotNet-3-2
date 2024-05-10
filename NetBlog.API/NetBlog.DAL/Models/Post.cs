using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.DAL.Models
{
    public class Post:IEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ContentPreview { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int CreatedBy {  get; set; }

        public IEnumerable<Comment> Comments { get; set; }
        public bool IsDeleted { get; set; }
    }
}
