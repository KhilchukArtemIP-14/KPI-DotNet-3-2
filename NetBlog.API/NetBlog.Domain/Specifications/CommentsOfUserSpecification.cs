using Ardalis.Specification;
using NetBlog.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Domain.Specifications
{
    public class CommentsOfUserSpecification:Specification<Comment>
    {
        public CommentsOfUserSpecification(string userId, bool orderByAscending = false) 
        {
            var query = orderByAscending ?
                Query.Where(c => c.AuthorId == userId).Include(c => c.Post).OrderBy(c => c.DateCreated) :
                Query.Where(c => c.AuthorId == userId).Include(c => c.Post).OrderByDescending(c => c.DateCreated);
        }
    }
}
