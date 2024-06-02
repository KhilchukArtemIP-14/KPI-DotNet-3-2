using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
    public class UpdatePostCommand : IRequest<PostDTO>
    {
        public Guid Id { get; set; }
        public UpdatePostDTO UpdatePostDTO { get; set; }
        public UpdatePostCommand(Guid id, UpdatePostDTO updatePostDTO)
        {
            Id = id;
            UpdatePostDTO = updatePostDTO;
        }
        public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, PostDTO>
        {
            private readonly IRepository<Post> _repository;
            private readonly IMapper _mapper;

            public UpdatePostCommandHandler(IRepository<Post> repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<PostDTO> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
            {
                var entity = await _repository.Get(request.Id);

                _mapper.Map(request.UpdatePostDTO, entity);

                var result = await _repository.Update(entity);
                return _mapper.Map<PostDTO>(result);
            }
        }
    }
}
