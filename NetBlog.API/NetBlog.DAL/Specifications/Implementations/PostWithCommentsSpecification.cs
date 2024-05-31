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
            if(commentsToFetch < 0) throw new Exception("Wrong comments number"); 
            AddInclude(p => p.Comments.Where(c => !c.IsDeleted).OrderByDescending(c=>c.DateCreated).Take(commentsToFetch));
        }
    }
}
