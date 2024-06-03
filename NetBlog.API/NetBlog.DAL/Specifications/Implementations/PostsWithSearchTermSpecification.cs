using NetBlog.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.DAL.Specifications.Implementations
{
    public class PostsWithSearchTermSpecification:BaseSpecification<Post>
    {
        public PostsWithSearchTermSpecification(string searchTerm, bool orderByAscending=false):base(p=>p.Title.Contains(searchTerm)||p.Content.Contains(searchTerm))
        {
            AddOrderBy(p => p.DateCreated, orderByAscending);
        }
    }
}
