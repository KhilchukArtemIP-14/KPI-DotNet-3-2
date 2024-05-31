using AutoMapper;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Identity;
using NetBlog.BAL.Services.UserSummaryService;
using NetBlog.BAL.DTO;
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
        private readonly IUserSummaryService _userSummaryService;


        public CommentService(IRepository<Comment> repository, IMapper mapper, IUserSummaryService userSummaryService)
        {
            _repository = repository;
            _mapper = mapper;
            _userSummaryService = userSummaryService;
        }

        public async Task<CommentDTO> CreateComment(CreateCommentDTO dto)
        {
            var entity = _mapper.Map<Comment>(dto);
            entity = await _repository.Add(entity);

            var result = _mapper.Map<CommentDTO>(entity);
            result.CreatedBy = await _userSummaryService.GetUserShortcut(result.CreatedBy.UserId);
            return result;
        }

        public async Task<CommentDTO> DeleteComment(Guid commentId)
        {
            var result = await _repository.Delete(commentId);

            return _mapper.Map<CommentDTO>(result);
        }

        public async Task<CommentDTO> GetCommentById(Guid id)
        {
            var result = await _repository.Get(id);

            return _mapper.Map<CommentDTO>(result);
        }

        public async Task<List<CommentDTO>> GetCommentsForPost(Guid postId, int pageNumber = 1, int pageSize = 5, bool orderByDateAscending = false)
        {
            if (pageNumber < 1 || pageSize < 1) return null;
            var spec = new CommentsForPostSpecification(postId, orderByDateAscending);

            var comments = await _repository.GetAll(spec, pageNumber, pageSize);
            var result = _mapper.Map<List<CommentDTO>>(comments);

            foreach (var res in result)
            {
                res.CreatedBy = await _userSummaryService.GetUserShortcut(res.CreatedBy.UserId);
            }

            return result;
        }

        public async Task<List<CommentShortcutDTO>> GetCommentShortuctsOfUser(string userId, int pageNumber = 1, int pageSize = 5, bool orderByDateAscending = false)
        {
            if (pageNumber < 1 || pageSize < 1) return null;
            var spec = new CommentsOfUserSpecification(userId, orderByDateAscending);
            var data = await _repository.GetAll(spec, pageNumber, pageSize);

            var result = _mapper.Map<List<CommentShortcutDTO>>(data);

            return result;
        }
    }
}
