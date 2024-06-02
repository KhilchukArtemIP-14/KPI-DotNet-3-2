using Ardalis.Specification;
using NetBlog.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Domain.Specifications
{
    public class PostsOfUserSpecification:Specification<Post>
    {
        public PostsOfUserSpecification(string userId, bool orderByAscending = false) 
        {
            var query = orderByAscending ?
                Query.Where(p => p.CreatedBy == userId).OrderBy(c => c.DateCreated) :
                Query.Where(p => p.CreatedBy == userId).OrderByDescending(c => c.DateCreated);
        }
    }
}
