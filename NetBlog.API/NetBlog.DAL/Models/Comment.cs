using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.DAL.Models
{
    public class Comment:IEntity
    {
        public Guid Id { get; set; }
        public string AuthorId { get; set; }
        public string CommentText { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid PostId { get; set; }

        public Post Post { get; set; }
        public bool IsDeleted { get; set; }
    }
}
