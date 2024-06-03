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

namespace NetBlog.Application.Features.Comments.Commands
{
    public class DeleteCommentCommand : IRequest<CommentDTO>
    {
        public Guid CommentId { get; set; }

        public DeleteCommentCommand(Guid commentId)
        {
            CommentId = commentId;
        }

        public class Handler : IRequestHandler<DeleteCommentCommand, CommentDTO>
        {
            private readonly IRepository<Comment> _repository;
            private readonly IMapper _mapper;

            public Handler(IRepository<Comment> repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<CommentDTO> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
            {
                var result = await _repository.Delete(request.CommentId);
                return _mapper.Map<CommentDTO>(result);
            }
        }
    }

}
