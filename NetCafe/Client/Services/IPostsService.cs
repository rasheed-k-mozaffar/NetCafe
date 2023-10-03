using NetCafe.Shared;

namespace NetCafe.Client.Services;

public interface IPostsService
{
    Task<ApiResponse<IEnumerable<PostSummaryDto>>> GetAllPostsAsync();
    Task<ApiResponse<PostDto>> GetPostByIdAsync(Guid postId);
    Task<ApiResponse> DeletePostAsync(Guid postId);
    Task<ApiResponse> CreatePostAsync(PostCreateDto post);
    Task<ApiResponse> UpdatePostAsync(Guid postId, PostUpdateDto post);
}
