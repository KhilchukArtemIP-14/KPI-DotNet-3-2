using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetBlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NetBlog.Persistance.Context
{
    public class NetBlogDbContext : IdentityDbContext
    {
        public NetBlogDbContext(DbContextOptions<NetBlogDbContext> options) : base(options)
        { }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(new List<IdentityRole>()
            {
                new IdentityRole()
                {
                    Name="Reader",
                    NormalizedName="Reader".ToUpper(),
                    Id="772101b0-46d6-4422-b672-5e1aca8db961",
                    ConcurrencyStamp="772101b0-46d6-4422-b672-5e1aca8db961"
                },
                new IdentityRole()
                {
                    Name="Author",
                    NormalizedName="Author".ToUpper(),
                    Id="a249e775-cdf9-472c-bc53-955e306f0f98",
                    ConcurrencyStamp="a249e775-cdf9-472c-bc53-955e306f0f98"
                }
            });
        }
    }
}
