using AutoMapper;
using MediatR;
using NetBlog.Application.DTOs;
using NetBlog.Domain.Entity;
using NetBlog.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Application.Features.Posts.Commands
{
    public class CreatePostCommand : IRequest<PostDTO>
    {
        public CreatePostDTO CreatePostDTO { get; set; }

        public CreatePostCommand(CreatePostDTO createPostDTO)
        {
            CreatePostDTO = createPostDTO;
        }
    }
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, PostDTO>
    {
        private readonly IRepository<Post> _repository;
        private readonly IMapper _mapper;

        public CreatePostCommandHandler(IRepository<Post> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PostDTO> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Post>(request.CreatePostDTO);

            var res = await _repository.Add(entity);

            return _mapper.Map<PostDTO>(res);
        }
    }
}
