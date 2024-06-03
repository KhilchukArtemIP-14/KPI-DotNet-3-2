using NetBlog.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.DAL.Specifications.Implementations
{
    public class CommentsOfUserSpecification:BaseSpecification<Comment>
    {
        public CommentsOfUserSpecification(string userId, bool orderByAscending=false) :base(c=>c.AuthorId==userId) {
            AddInclude(c => c.Post);
            AddOrderBy(c => c.DateCreated, orderByAscending);
        }
    }
}
