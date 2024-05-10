using Microsoft.EntityFrameworkCore;
using NetBlog.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.DAL.Data
{
    public class NetBlogDbContext: DbContext
    {
        public NetBlogDbContext(DbContextOptions options):base(options) 
        { }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
