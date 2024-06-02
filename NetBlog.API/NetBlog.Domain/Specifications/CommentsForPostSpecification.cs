using Ardalis.Specification;
using NetBlog.Domain.Entity;


namespace NetBlog.Domain.Specifications
{
    public class CommentsForPostSpecification:Specification<Comment>
    {
        public CommentsForPostSpecification(Guid postId, bool orderByAscending = false)
        {
            var query = orderByAscending? 
                Query.Where(c => c.PostId == postId).OrderBy(c => c.DateCreated):
                Query.Where(c => c.PostId == postId).OrderByDescending(c => c.DateCreated);
        }
    }
}
