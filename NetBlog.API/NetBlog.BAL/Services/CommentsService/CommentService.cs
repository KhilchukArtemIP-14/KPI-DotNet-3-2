using AutoMapper;
using Microsoft.AspNetCore.DataProtection.Repositories;
using NetBlog.Common.DTO;
using NetBlog.DAL.Models;
using NetBlog.DAL.Repositories;
using NetBlog.DAL.Specifications.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.BAL.Services.CommentsService
{
    public class CommentService : ICommentsService
    {
        private readonly IRepository<Comment> _repository;
        private readonly IMapper _mapper;

        public CommentService(IRepository<Comment> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CommentDTO> CreateComment(CreateCommentDTO dto)
        {
            var entity = _mapper.Map<Comment>(dto);

            var result = await _repository.Add(entity);

            return _mapper.Map<CommentDTO>(result);
        }

        public async Task<CommentDTO> DeleteComment(Guid commentId)
        {
            var result = await _repository.Delete(commentId);

            return _mapper.Map<CommentDTO>(result);
        }

        public async Task<List<CommentDTO>> GetCommentsForPost(Guid postId)
        {
            var spec = new CommentsForPostSpecification(postId);

            var result = await _repository.GetAll(spec);

            return _mapper.Map<List<CommentDTO>>(result);
        }
    }
}
