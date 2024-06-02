using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using NetBlog.Application.DTOs;
using NetBlog.Application.Features.UserSummary.Queries;
using NetBlog.Domain.Entity;
using NetBlog.Domain.RepositoryContracts;
using NetBlog.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Application.Features.Posts.Queries
{
    public class GetPostByIdQuery : IRequest<PostDTO>
    {
        public Guid Id { get; set; }
        public int CommentsToLoad { get; set; }
        public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, PostDTO>
        {
            private readonly IRepository<Post> _repository;
            private readonly IMapper _mapper;
            private readonly UserManager<IdentityUser> _userManager;

            public GetPostByIdQueryHandler(IRepository<Post> repository, IMapper mapper, UserManager<IdentityUser> userManager)
            {
                _repository = repository;
                _mapper = mapper;
                _userManager = userManager;
            }

            public async Task<PostDTO> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
            {
                if (request.CommentsToLoad < 1) return null;
                var spec = new PostWithCommentsSpecification(request.CommentsToLoad);
                var entity = await _repository.Get(request.Id, spec);

                var result = _mapper.Map<PostDTO>(entity);
                if (result != null)
                {
                    result.CreatedBy = await GetUserShortcut(result.CreatedBy.UserId);
                    foreach (var comment in result.Comments)
                    {
                        comment.CreatedBy = await GetUserShortcut(comment.CreatedBy.UserId);
                    }
                }

                return result;
            }
            private async Task<UserShortcutDTO> GetUserShortcut(string id)
            {
                var user = await _userManager.FindByIdAsync(id);

                var name = user != null ? user.UserName : "NOT FOUND";

                return new UserShortcutDTO
                {
                    UserId = id,
                    UserName = name
                };
            }
        }
    }

}
