namespace NetCafe.Client.Services;

public interface ITagsService
{
    Task<ApiResponse<IEnumerable<TagSummaryDto>>> GetAllTagsAsync();
    Task<ApiResponse<TagDto>> GetTagByIdAsync(Guid tagId);
    Task<ApiResponse> DeleteTagAsync(Guid tagId);
    Task<ApiResponse> CreateTagAsync(TagCreateDto tag);
    Task<ApiResponse> UpdateTagAsync(Guid tagId, TagUpdateDto tag);
}
