namespace NetCafe.Client.Services;

public interface ICommentsService
{
    Task<ApiResponse<IEnumerable<CommentDto>>> GetCommentsForPostAsync(Guid postId);
    Task<ApiResponse<CommentDto>> GetCommentByIdAsync(Guid commentId);
    Task<ApiResponse> AddCommentAsync(Guid postId, CommentCreateDto comment);
    Task<ApiResponse> AddReplyToCommentAsync(Guid commentId, CommentCreateDto reply);
    Task<ApiResponse> DeleteCommentAsync(Guid commentId);
    Task<ApiResponse> UpdateCommentAsync(Guid commentId, CommentUpdateDto comment);
}
