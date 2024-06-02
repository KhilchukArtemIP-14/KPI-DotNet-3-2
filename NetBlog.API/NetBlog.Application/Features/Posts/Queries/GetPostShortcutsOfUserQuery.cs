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

namespace NetBlog.Application.Features.Posts.Queries
{
    public class GetPostShortcutsOfUserQuery : IRequest<List<PostShortcutDTO>>
    {
        public string UserId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool OrderByDateAscending { get; set; }
        public GetPostShortcutsOfUserQuery(string userId, int pageNumber, int pageSize, bool orderByDateAscending)
        {
            UserId = userId;
            PageNumber = pageNumber;
            PageSize = pageSize;
            OrderByDateAscending = orderByDateAscending;
        }
        public class GetPostShortcutsOfUserQueryHandler : IRequestHandler<GetPostShortcutsOfUserQuery, List<PostShortcutDTO>>
        {
            private readonly IRepository<Post> _repository;
            private readonly IMapper _mapper;

            public GetPostShortcutsOfUserQueryHandler(IRepository<Post> repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<List<PostShortcutDTO>> Handle(GetPostShortcutsOfUserQuery request, CancellationToken cancellationToken)
            {
                if (request.PageNumber < 1 || request.PageSize < 1) return null;
                var spec = new PostsOfUserSpecification(request.UserId, request.OrderByDateAscending);
                var data = await _repository.GetAll(spec, request.PageNumber, request.PageSize);

                return _mapper.Map<List<PostShortcutDTO>>(data);
            }
        }
    }
}
