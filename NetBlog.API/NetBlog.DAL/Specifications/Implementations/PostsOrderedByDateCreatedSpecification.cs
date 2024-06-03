using NetBlog.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.DAL.Specifications.Implementations
{
    public class PostsOrderedByDateCreatedSpecification:BaseSpecification<Post>
    {
        public PostsOrderedByDateCreatedSpecification(bool orderByAscending=false) {
            AddOrderBy(p => p.DateCreated, orderByAscending);
        }
    }
}
