using AutoMapper;
using Lab2_WebAPI_v4.Core.Services.Interfaces;
using Lab2_WebAPI_v4.Data.Entities;
using Lab2_WebAPI_v4.Data.Interfaces;
using Lab2_WebAPI_v4.DTOs.Post;

namespace Lab2_WebAPI_v4.Core.Services
{
    /// <summary>
    /// Handles business logic related to blog posts.
    /// Responsible for validation and mapping between DTOs and entities.
    /// Communicates with repository layer.
    /// </summary>
    public class PostService : IPostService
    {
        private readonly IPostRepo _repo;
        private readonly IMapper _mapper;
        private readonly ICategoryRepo _categoryRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostService"/> class.
        /// </summary>
        /// <param name="repo">Injected post repository.</param>
        /// <param name="mapper">Injected AutoMapper instance.</param>
        /// <param name="categoryRepo">Injected category repository (used for validation).</param>
        public PostService(IPostRepo repo, IMapper mapper, ICategoryRepo categoryRepo)
        {
            _repo = repo;
            _mapper = mapper;
            _categoryRepo = categoryRepo;
        }

        // -------------------- GET ALL POSTS --------------------

        /// <summary>
        /// Retrieves all posts from the database.
        /// Maps Post entities to PostDto objects.
        /// </summary>
        /// <returns>List of PostDto.</returns>
        public async Task<List<PostDto>> GetAllAsync()
        {
            var posts = await _repo.GetAllPostsAsync();

            // Map Entity → DTO
            return _mapper.Map<List<PostDto>>(posts);
        }

        // -------------------- CREATE POST --------------------

        /// <summary>
        /// Creates a new post for the authenticated user.
        /// Validates that the selected category exists.
        /// </summary>
        /// <param name="dto">Post creation data.</param>
        /// <param name="userId">Authenticated user's ID (from JWT).</param>
        /// <returns>Created PostDto.</returns>
        public async Task<PostDto> AddAsync(CreatePostDto dto, int userId)
        {
            // Validate category existence
            if (!await _categoryRepo.CategoryExistsAsync(dto.CategoryID))
                throw new ArgumentException("Category does not exist.");

            // Map DTO → Entity
            var post = _mapper.Map<Post>(dto);

            // Assign correct owner
            post.UserID = userId;

            await _repo.AddPostAsync(post);

            // Map saved Entity → DTO
            return _mapper.Map<PostDto>(post);
        }

        // -------------------- UPDATE POST --------------------

        /// <summary>
        /// Updates an existing post.
        /// Only the post owner can update it (validated in repository).
        /// Also validates category existence.
        /// </summary>
        /// <param name="dto">Updated post data.</param>
        /// <param name="userId">Authenticated user's ID.</param>
        /// <returns>True if update succeeded, otherwise false.</returns>
        public async Task<bool> UpdateAsync(UpdatePostDto dto, int userId)
        {
            // Validate category existence
            if (!await _categoryRepo.CategoryExistsAsync(dto.CategoryID))
                throw new ArgumentException("Category does not exist.");

            // Map DTO → Entity
            var post = _mapper.Map<Post>(dto);

            // Ensure correct ownership
            post.UserID = userId;

            return await _repo.UpdatePostAsync(post);
        }

        // -------------------- DELETE POST --------------------

        /// <summary>
        /// Deletes a post.
        /// Only the post owner can delete it (validated in repository).
        /// </summary>
        /// <param name="postId">ID of the post.</param>
        /// <param name="userId">Authenticated user's ID.</param>
        /// <returns>True if deletion succeeded, otherwise false.</returns>
        public async Task<bool> DeleteAsync(int postId, int userId)
        {
            return await _repo.DeletePostAsync(postId, userId);
        }

        // -------------------- SEARCH BY TITLE --------------------

        /// <summary>
        /// Searches posts by title using partial matching.
        /// </summary>
        /// <param name="title">Search keyword.</param>
        /// <returns>List of matching PostDto objects.</returns>
        public async Task<List<PostDto>> SearchByTitleAsync(string title)
        {
            var posts = await _repo.SearchByTitleAsync(title);

            return _mapper.Map<List<PostDto>>(posts);
        }

        // -------------------- SEARCH BY CATEGORY --------------------

        /// <summary>
        /// Retrieves posts belonging to a specific category.
        /// </summary>
        /// <param name="categoryId">Category ID.</param>
        /// <returns>List of PostDto objects.</returns>
        public async Task<List<PostDto>> SearchByCategoryAsync(int categoryId)
        {
            var posts = await _repo.SearchByCategoryAsync(categoryId);

            return _mapper.Map<List<PostDto>>(posts);
        }
    }
}