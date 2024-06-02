using Ardalis.Specification;
using NetBlog.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Domain.Specifications
{
    public class PostsWithSearchTermSpecification:Specification<Post>
    {
        public PostsWithSearchTermSpecification(string searchTerm, bool orderByAscending = false) 
        {
            var query = orderByAscending ?
                Query.Where(p => p.Title.Contains(searchTerm) || p.Content.Contains(searchTerm)).OrderBy(c => c.DateCreated) :
                Query.Where(p => p.Title.Contains(searchTerm) || p.Content.Contains(searchTerm)).OrderByDescending(c => c.DateCreated);
        }
    }
}
