using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.DAL.Models
{
    public class Post:IEntity
    {
        [Required]
        public Guid Id { get; set; }
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
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        [Required]
        public string CreatedBy {  get; set; }

        public IEnumerable<Comment> Comments { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
    }
}
