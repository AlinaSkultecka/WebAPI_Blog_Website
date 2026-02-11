using Lab2_WebAPI_v4.Core.Services.Interfaces;
using Lab2_WebAPI_v4.Data.Entities;
using Lab2_WebAPI_v4.Data.Interfaces;
using Lab2_WebAPI_v4.DTOs;
using Lab2_WebAPI_v4.DTOs.Post;
using AutoMapper;

namespace Lab2_WebAPI_v4.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepo _repo;
        private readonly IMapper _mapper;

        // Only ONE constructor
        public PostService(IPostRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // -------------------- GET ALL --------------------

        public async Task<List<PostDto>> GetAllAsync()
        {
            var posts = await _repo.GetAllPostsAsync();

            // Convert Entity → DTO
            return _mapper.Map<List<PostDto>>(posts);
        }

        // -------------------- ADD --------------------

        public async Task<PostDto> AddAsync(CreatePostDto dto, int userId)
        {
            var post = _mapper.Map<Post>(dto);

            post.UserID = userId;

            await _repo.AddPostAsync(post);

            // Now post has generated PostID
            return _mapper.Map<PostDto>(post);
        }


        // -------------------- UPDATE --------------------

        public async Task<bool> UpdateAsync(PostDto dto, int userId)
        {
            var post = _mapper.Map<Post>(dto);

            post.UserID = userId;

            return await _repo.UpdatePostAsync(post);
        }

        // -------------------- DELETE --------------------

        public async Task<bool> DeleteAsync(int postId, int userId)
        {
            return await _repo.DeletePostAsync(postId, userId);
        }

        // -------------------- SEARCH BY TITLE --------------------

        public async Task<List<PostDto>> SearchByTitleAsync(string title)
        {
            var posts = await _repo.SearchByTitleAsync(title);

            return _mapper.Map<List<PostDto>>(posts);
        }

        // -------------------- SEARCH BY CATEGORY --------------------

        public async Task<List<PostDto>> SearchByCategoryAsync(int categoryId)
        {
            var posts = await _repo.SearchByCategoryAsync(categoryId);

            return _mapper.Map<List<PostDto>>(posts);
        }
    }
}

