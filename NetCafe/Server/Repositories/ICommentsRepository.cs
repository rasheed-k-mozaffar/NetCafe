namespace NetCafe.Server.Repositories;

public interface ICommentsRepository
{
    Task<ICollection<Comment>> GetCommentsAsync(Guid postId);
    Task<Comment> GetCommentAsync(Guid commentId);
    Task<bool> AddCommentAsync(Guid postId, Comment comment);
    Task<bool> DeleteCommentAsync(Guid commentId);
    // Task<bool> AddReplyToCommentAsync(Guid commentId, Comment reply);
    Task<bool> UpdateCommentAsync(Guid commentId, Comment comment);
}
