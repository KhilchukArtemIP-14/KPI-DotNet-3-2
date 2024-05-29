using NetBlog.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.DAL.Specifications.Implementations
{
    public class CommentsForPostSpecification:BaseSpecification<Comment>
    {
        public CommentsForPostSpecification(Guid postId, bool orderByAscending = false):base(c=>c.PostId== postId) 
        {
            AddOrderBy(c=>c.DateCreated, orderByAscending);
        }
    }
}
