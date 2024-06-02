using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Emit;
using NetBlog.API;
using NetBlog.Persistance.Context;
using NetBlog.Domain.Entity;

namespace NetBlog.IntegrationTests.Utilities
{
    public class AppFactory: WebApplicationFactory<Program>
    {

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<NetBlogDbContext>));

                if (dbContextDescriptor != null)
                {
                    services.Remove(dbContextDescriptor);
                }

                services.AddDbContext<NetBlogDbContext>(options =>
                {
                    options.UseInMemoryDatabase("NetBlog");
                });
            });
        }
        public async Task SeedDataAsync()
        {
            using (var scope = Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var context = scopedServices.GetRequiredService<NetBlogDbContext>();

                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();

                // Adding author user
                var author = new IdentityUser()
                {
                    Id = "3c8b0d12-13e9-4f42-85a4-5d3ce1e7e34f",
                    UserName = "Author",
                    Email = "frookt4555@gmail.com",
                    NormalizedEmail = "frookt4555@gmail.com".ToUpper(),
                    NormalizedUserName = "Author".ToUpper()
                };
                author.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(author, "SuperSecurePaswwordqwerty@");
                await context.Set<IdentityUser>().AddAsync(author);
                await context.SaveChangesAsync();

                var authorClaim = new IdentityUserClaim<string>
                {
                    UserId = author.Id,
                    ClaimType = "bio",
                    ClaimValue = "Empty"
                };

                context.UserClaims.Add(authorClaim);
                await context.SaveChangesAsync();


                var authorRole = new List<IdentityUserRole<string>>()
                {
                    new ()
                    {
                        UserId = "3c8b0d12-13e9-4f42-85a4-5d3ce1e7e34f",
                        RoleId = "a249e775-cdf9-472c-bc53-955e306f0f98" // author role
                    }
                };
                await context.Set<IdentityUserRole<string>>().AddRangeAsync(authorRole);
                await context.SaveChangesAsync();

                // adding reader user
                var reader = new IdentityUser()
                {
                    Id = "4c8b0d12-13e9-4f42-85a4-5d3ce1e7e34f",
                    UserName = "Reader",
                    Email = "frookt4444@gmail.com",
                    NormalizedEmail = "frookt4444@gmail.com".ToUpper(),
                    NormalizedUserName = "Reader".ToUpper()
                };
                reader.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(reader, "SuperSecurePaswwordqwerty@");
                await context.Set<IdentityUser>().AddAsync(reader);
                await context.SaveChangesAsync();

                var readerClaim = new IdentityUserClaim<string>
                {
                    UserId = reader.Id,
                    ClaimType = "bio",
                    ClaimValue = "Empty"
                };

                context.UserClaims.Add(readerClaim);
                await context.SaveChangesAsync();

                var readerRole = new List<IdentityUserRole<string>>()
                {
                    new ()
                    {
                        UserId = "4c8b0d12-13e9-4f42-85a4-5d3ce1e7e34f",
                        RoleId = "772101b0-46d6-4422-b672-5e1aca8db961" // reader role
                    }
                };
                await context.Set<IdentityUserRole<string>>().AddRangeAsync(readerRole);
                await context.SaveChangesAsync();

                // Adding posts
                var posts = new List<Post>
                {
                    new Post
                    {
                        Id = Guid.Parse("f6536d0a-7e56-4a1e-9278-f1d5cc95e0b2"),
                        Title = "Post1",
                        ContentPreview = "preview1",
                        Content = "content1",
                        DateCreated = DateTime.Now,
                        DateUpdated = null,
                        CreatedBy = "3c8b0d12-13e9-4f42-85a4-5d3ce1e7e34f",
                        Comments = new List<Comment>
                        {
                            new Comment
                            {
                                Id = Guid.Parse("3c724b9c-0452-452e-a738-cfd52e586b39"),
                                CommentText = "Nice post!",
                                AuthorId = "3c8b0d12-13e9-4f42-85a4-5d3ce1e7e34f",
                                DateCreated = DateTime.Now
                            },
                            new Comment
                            {
                                Id = Guid.Parse("69a06e65-7598-4dbf-99cd-218ab5975456"),
                                CommentText = "Great job!",
                                AuthorId = "4c8b0d12-13e9-4f42-85a4-5d3ce1e7e34f",
                                DateCreated = DateTime.Now.AddHours(1)
                            }
                        },
                        IsDeleted = false
                    },
                    new Post
                    {
                        Id = Guid.Parse("9ff826d3-c2cd-4e25-bb46-fb07bc693d0a"),
                        Title = "Post2",
                        ContentPreview = "Preview2",
                        Content = "Content2",
                        DateCreated = DateTime.Now.AddDays(-1),
                        DateUpdated = DateTime.Now.AddDays(-0.5),
                        CreatedBy = "3c8b0d12-13e9-4f42-85a4-5d3ce1e7e34f",
                        Comments = new List<Comment>
                        {
                            new Comment
                            {
                                Id = Guid.Parse("b1e3aa8d-7246-4b7b-9ff1-c2a1efab20f1"),
                                CommentText = "Interesting read!",
                                AuthorId = "4c8b0d12-13e9-4f42-85a4-5d3ce1e7e34f",
                                DateCreated = DateTime.Now.AddDays(-1).AddHours(3)
                            }
                        },
                        IsDeleted = false
                    }
                };
                await context.Posts.AddRangeAsync(posts);
                await context.SaveChangesAsync();
            }
        }
    }
}
