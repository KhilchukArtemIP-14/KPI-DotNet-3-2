using NetBlog.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.DAL.Specifications.Implementations
{
    public class PostWithCommentsSpecification:BaseSpecification<Post>
    {
        public PostWithCommentsSpecification(int commentsToFetch=5):base(){
            AddInclude(p => p.Comments.Where(c => !c.IsDeleted).OrderByDescending(c=>c.DateCreated).Take(commentsToFetch));
        }
    }
}
