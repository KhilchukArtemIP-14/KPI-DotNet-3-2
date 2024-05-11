using AutoMapper;
using NetBlog.Common.DTO;
using NetBlog.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Common.Mappings
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<Comment, CommentDTO>()
                 .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => new UserShortcutDTO { UserId = src.AuthorId }));
            CreateMap<Post, PostDTO>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => new UserShortcutDTO { UserId = src.CreatedBy }))
                .ForMember(p=>p.Comments, opt=>opt.MapFrom(src=>src.Comments.Where(c=>!c.IsDeleted)));
            CreateMap<Post, PostSummaryDTO>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => new UserShortcutDTO { UserId = src.CreatedBy }));
            CreateMap<CreateCommentDTO, Comment>().
                ForMember(c=>c.DateCreated, opt=>opt.MapFrom(_=>DateTime.Now));
            CreateMap<CreatePostDTO, Post>()
                .ForMember(c => c.DateCreated, opt => opt.MapFrom(_ => DateTime.Now));
            CreateMap<UpdatePostDTO, Post>()
                .ForMember(c => c.DateUpdated, opt => opt.MapFrom(_ => DateTime.Now));
        }
    }
}
