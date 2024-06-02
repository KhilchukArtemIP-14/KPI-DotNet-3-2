using AutoMapper;
using MediatR;
using NetBlog.Application.DTOs;
using NetBlog.Domain.Entity;
using NetBlog.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Application.Features.Posts.Commands
{
    public class DeletePostCommand : IRequest<PostDTO>
    {
        public Guid Id { get; set; }
        public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, PostDTO>
        {
            private readonly IRepository<Post> _repository;
            private readonly IMapper _mapper;
            public DeletePostCommandHandler(IRepository<Post> repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<PostDTO> Handle(DeletePostCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.Delete(request.Id);

                return _mapper.Map<PostDTO>(result);
            }
        }
    }
}
