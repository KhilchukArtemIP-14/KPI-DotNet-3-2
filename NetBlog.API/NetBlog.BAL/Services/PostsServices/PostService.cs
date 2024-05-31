using AutoMapper;
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

namespace NetBlog.BAL.Services.PostsServices
{
    public class PostService : IPostService
    {
        private readonly IRepository<Post> _repository;
        private readonly IMapper _mapper;
        private readonly IUserSummaryService _userSummaryService;
        public PostService(IRepository<Post> repository, IMapper mapper, IUserSummaryService userSummaryService)
        {
            _repository = repository;
            _mapper = mapper;
            _userSummaryService = userSummaryService;
        }

        public async Task<PostDTO> Add(CreatePostDTO dto)
        {
            var entity  = _mapper.Map<Post>(dto);

            var result = await _repository.Add(entity);

            return _mapper.Map<PostDTO>(result);
        }

        public async Task<PostDTO> Delete(Guid id)
        {
            var result = await _repository.Delete(id);

            return _mapper.Map<PostDTO>(result);
        }

        public async Task<PostDTO> GetById(Guid id, int commentsToLoad = 5)
        {
            if (commentsToLoad < 1) return null;
            var spec = new PostWithCommentsSpecification(commentsToLoad);
            var entity = await _repository.Get(id, spec);

            var result = _mapper.Map<PostDTO>(entity);
            if (result != null)
            {
                result.CreatedBy = await _userSummaryService.GetUserShortcut(result.CreatedBy.UserId);
                foreach (var comment in result.Comments)
                {
                    comment.CreatedBy = await _userSummaryService.GetUserShortcut(comment.CreatedBy.UserId);
                }
            }

            return result;
        }

        public async Task<List<PostShortcutDTO>> GetPostShortcutsOfUser(string userId, int pageNumber = 1, int pageSize = 5, bool orderByDateAscending = false)
        {
            if (pageNumber < 1 || pageSize < 1) return null;
            var spec = new PostsOfUserSpecification(userId, orderByDateAscending);
            var data = await _repository.GetAll(spec, pageNumber, pageSize);

            return _mapper.Map<List<PostShortcutDTO>>(data);
        }

        public async Task<List<PostSummaryDTO>> GetSummaries(int pageNumber = 1, int pageSize = 5, string? searchTerm = null, bool orderByDateAscending = false)
        {
            if (pageNumber < 1 || pageSize < 1) return null;
            BaseSpecification<Post> spec = searchTerm != null ?
                new PostsWithSearchTermSpecification(searchTerm, orderByDateAscending):
                new PostsOrderedByDateCreatedSpecification(orderByDateAscending);

            var entitites = await _repository.GetAll(spec, pageNumber, pageSize);

            var result = _mapper.Map<List<PostSummaryDTO>>(entitites);
            foreach(var res in result)
            {
                res.CreatedBy = await _userSummaryService.GetUserShortcut(res.CreatedBy.UserId);
            }

            return result;
        }

        public async Task<PostDTO> Update(Guid id, UpdatePostDTO dto)
        {
            var entity = await _repository.Get(id);

            _mapper.Map(dto, entity);

            var result = await _repository.Update(entity);
            return _mapper.Map<PostDTO>(result);
        }
    }
}
