using Ardalis.Specification;
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
    public class GetPostSummariesQuery : IRequest<List<PostSummaryDTO>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? SearchQuery { get; set; }
        public bool OrderByDateAscending { get; set; }
        public class GetPostSummariesQueryHandler : IRequestHandler<GetPostSummariesQuery, List<PostSummaryDTO>>
        {
            private readonly IRepository<Post> _repository;
            private readonly IMapper _mapper;
            private readonly UserManager<IdentityUser> _userManager;

            public GetPostSummariesQueryHandler(IRepository<Post> repository, IMapper mapper, UserManager<IdentityUser> userManager)
            {
                _repository = repository;
                _mapper = mapper;
                _userManager = userManager;
            }

            public async Task<List<PostSummaryDTO>> Handle(GetPostSummariesQuery request, CancellationToken cancellationToken)
            {
                if (request.PageNumber < 1 || request.PageSize < 1) return null;
                Specification<Post> spec = request.SearchQuery != null ?
                    new PostsWithSearchTermSpecification(request.SearchQuery, request.OrderByDateAscending) :
                    new PostsOrderedByDateCreatedSpecification(request.OrderByDateAscending);

                var entitites = await _repository.GetAll(spec, request.PageNumber, request.PageSize);

                var result = _mapper.Map<List<PostSummaryDTO>>(entitites);
                foreach (var res in result)
                {
                    res.CreatedBy = await GetUserShortcut(res.CreatedBy.UserId);
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
