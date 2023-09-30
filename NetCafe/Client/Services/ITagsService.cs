namespace NetCafe.Client.Services;

public interface ITagsService
{
    Task<ApiResponse<ICollection<TagSummaryDto>>> GetAllTagsAsync();
    Task<ApiResponse<PostDto>> GetTagByIdAsync(Guid tagId);
    Task<ApiResponse> DeleteTagAsync(Guid tagId);
    Task<ApiResponse> CreateTagAsync(TagCreateDto tag);
    Task<ApiResponse> UpdateTagAsync(PostUpdateDto tag);
}
