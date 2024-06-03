using Ardalis.Specification;
using NetBlog.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Domain.Specifications
{
    public class PostsOrderedByDateCreatedSpecification:Specification<Post>
    {
        public PostsOrderedByDateCreatedSpecification(bool orderByAscending = false) 
        {
            var query = orderByAscending ?
                Query.OrderBy(c => c.DateCreated) :
                Query.OrderByDescending(c => c.DateCreated);
        }
    }
}
