using Ardalis.Specification;
using NetBlog.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Domain.Specifications
{
    public class PostWithCommentsSpecification:Specification<Post>
    {
        public PostWithCommentsSpecification(int commentsToFetch = 5) 
        {
            if (commentsToFetch < 0) throw new Exception("Wrong comments number");
            var query = Query.Include(p => p.Comments.Where(c => !c.IsDeleted).OrderByDescending(c => c.DateCreated).Take(commentsToFetch));
        }
    }
}
