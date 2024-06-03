using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using NetBlog.Application.DTOs;
using NetBlog.Domain.Entity;
using NetBlog.Domain.RepositoryContracts;
using NetBlog.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Application.Features.Comments.Queries
{
    public class GetCommentsForPostQuery : IRequest<List<CommentDTO>>
    {
        public Guid PostId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool OrderByDateAscending { get; set; }

        public GetCommentsForPostQuery(Guid postId, int pageNumber, int pageSize, bool orderByDateAscending)
        {
            PostId = postId;
            PageNumber = pageNumber;
            PageSize = pageSize;
            OrderByDateAscending = orderByDateAscending;
        }

        public class Handler : IRequestHandler<GetCommentsForPostQuery, List<CommentDTO>>
        {
            private readonly IRepository<Comment> _repository;
            private readonly IMapper _mapper;
            private readonly UserManager<IdentityUser> _userManager;

            public Handler(IRepository<Comment> repository, IMapper mapper, UserManager<IdentityUser> userManager)
            {
                _repository = repository;
                _mapper = mapper;
                _userManager = userManager;
            }

            public async Task<List<CommentDTO>> Handle(GetCommentsForPostQuery request, CancellationToken cancellationToken)
            {
                if (request.PageNumber < 1 || request.PageSize < 1) return null;
                var spec = new CommentsForPostSpecification(request.PostId, request.OrderByDateAscending);

                var comments = await _repository.GetAll(spec, request.PageNumber, request.PageSize);
                var result = _mapper.Map<List<CommentDTO>>(comments);

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
