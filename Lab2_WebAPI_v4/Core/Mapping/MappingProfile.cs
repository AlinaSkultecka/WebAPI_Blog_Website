using AutoMapper;
using Lab2_WebAPI_v4.Data.Entities;
using Lab2_WebAPI_v4.DTOs;
using Lab2_WebAPI_v4.DTOs.Post;

namespace Lab2_WebAPI_v4.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entity → DTO
            CreateMap<Post, PostDto>();

            // DTO → Entity
            CreateMap<CreatePostDto, Post>();
        }
    }
}
