using AutoMapper;
using Lab2_WebAPI_v4.Data.DTOs.Post;
using Lab2_WebAPI_v4.Data.Entities;

namespace Lab2_WebAPI_v4.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entity ↔ DTO
            CreateMap<Post, PostDto>().ReverseMap();

            // Create DTO → Entity
            CreateMap<CreatePostDto, Post>();

            // If you want to use UpdatePostDto instead of PostDto:
            CreateMap<UpdatePostDto, Post>();
        }
    }
}
