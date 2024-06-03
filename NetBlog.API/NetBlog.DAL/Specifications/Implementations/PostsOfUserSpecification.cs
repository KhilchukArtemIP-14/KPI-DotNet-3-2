using NetBlog.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.DAL.Specifications.Implementations
{
    public class PostsOfUserSpecification:BaseSpecification<Post>
    {
        public PostsOfUserSpecification(string userId, bool orderByAscending = false):base(p=>p.CreatedBy==userId) {
            AddOrderBy(p=>p.DateCreated, orderByAscending);
        }
    }
}
