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
        [Required]
        [MinLength(1)]
        public string Title { get; set; }
        [Required]
        [MinLength(1)]
        public string ContentPreview { get; set; }
        [Required]
        [MinLength(1)]
        public string Content { get; set; }
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

                _mapper.Map(request, entity);

                var result = await _repository.Update(entity);
                return _mapper.Map<PostDTO>(result);
            }
        }
    }
}
