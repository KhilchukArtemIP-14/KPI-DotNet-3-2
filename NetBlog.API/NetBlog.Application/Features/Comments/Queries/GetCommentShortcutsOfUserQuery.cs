using AutoMapper;
using MediatR;
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
    public class GetCommentShortcutsOfUserQuery : IRequest<List<CommentShortcutDTO>>
    {
        public string UserId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool OrderByDateAscending { get; set; }

        public GetCommentShortcutsOfUserQuery(string userId, int pageNumber, int pageSize, bool orderByDateAscending)
        {
            UserId = userId;
            PageNumber = pageNumber;
            PageSize = pageSize;
            OrderByDateAscending = orderByDateAscending;
        }

        public class Handler : IRequestHandler<GetCommentShortcutsOfUserQuery, List<CommentShortcutDTO>>
        {
            private readonly IRepository<Comment> _repository;
            private readonly IMapper _mapper;

            public Handler(IRepository<Comment> repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<List<CommentShortcutDTO>> Handle(GetCommentShortcutsOfUserQuery request, CancellationToken cancellationToken)
            {
                if (request.PageNumber < 1 || request.PageSize < 1) return null;
                var spec = new CommentsOfUserSpecification(request.UserId, request.OrderByDateAscending);
                var data = await _repository.GetAll(spec, request.PageNumber, request.PageSize);

                var result = _mapper.Map<List<CommentShortcutDTO>>(data);
                return result;
            }
        }
    }

}
