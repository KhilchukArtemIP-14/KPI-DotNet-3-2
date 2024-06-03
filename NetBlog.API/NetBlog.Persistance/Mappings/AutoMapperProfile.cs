using AutoMapper;
using NetBlog.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBlog.Application.DTOs;
using NetBlog.Application.Features.Posts.Commands;

namespace NetBlog.Persistance.Mappings
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Comment, CommentDTO>()
                 .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => new UserShortcutDTO { UserId = src.AuthorId }));
            CreateMap<Comment, CommentShortcutDTO>()
                .ForMember(dest => dest.PostTitle, opt => opt.MapFrom(c => c.Post.Title))
                .ForMember(dest => dest.PostId, opt => opt.MapFrom(c => c.Post.Id));
            CreateMap<Post, PostDTO>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => new UserShortcutDTO { UserId = src.CreatedBy }))
                .ForMember(p => p.Comments, opt => opt.MapFrom(src => src.Comments.Where(c => !c.IsDeleted)));
            CreateMap<Post, PostSummaryDTO>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => new UserShortcutDTO { UserId = src.CreatedBy }));
            CreateMap<Post, PostShortcutDTO>();
            CreateMap<CreateCommentDTO, Comment>().
                ForMember(c => c.DateCreated, opt => opt.MapFrom(_ => DateTime.Now));
            CreateMap<CreatePostDTO, Post>()
                .ForMember(c => c.DateCreated, opt => opt.MapFrom(_ => DateTime.Now));
            CreateMap<UpdatePostDTO, Post>()
                .ForMember(c => c.DateUpdated, opt => opt.MapFrom(_ => DateTime.Now));
        }
    }
}
