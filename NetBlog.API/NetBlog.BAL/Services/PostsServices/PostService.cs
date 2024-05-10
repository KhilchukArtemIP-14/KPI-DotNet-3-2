using AutoMapper;
using NetBlog.Common.DTO;
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

        public PostService(IRepository<Post> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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

        public async Task<PostDTO> GetById(Guid id)
        {
            var spec = new PostWithCommentsSpecification();

            var result = await _repository.Get(id, spec);

            return _mapper.Map<PostDTO>(result);
        }

        public async Task<List<PostSummaryDTO>> GetSummaries()
        {
            var result = await _repository.GetAll();

            return _mapper.Map<List<PostSummaryDTO>>(result);
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
