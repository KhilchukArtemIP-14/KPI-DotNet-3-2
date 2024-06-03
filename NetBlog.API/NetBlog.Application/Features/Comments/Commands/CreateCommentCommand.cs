using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
    public class CreateCommentCommand : IRequest<CommentDTO>
    {
        public CreateCommentDTO Dto { get; set; }

        public CreateCommentCommand(CreateCommentDTO dto)
        {
            Dto = dto;
        }

        public class Handler : IRequestHandler<CreateCommentCommand, CommentDTO>
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

            public async Task<CommentDTO> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<Comment>(request.Dto);
                entity = await _repository.Add(entity);

                var result = _mapper.Map<CommentDTO>(entity);
                result.CreatedBy = await GetUserShortcut(result.CreatedBy.UserId);
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
