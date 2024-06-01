using NetBlog.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Domain.Entities
{
    public class Comment : IEntity
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string AuthorId { get; set; }
        [Required]
        [MinLength(1)]
        public string CommentText { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public Guid PostId { get; set; }

        public Post Post { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
    }
}
