namespace NetCafe.Server.Repositories;

public interface IPostsRepository
{
    Task<ICollection<Post>> GetPostsAsync();
    Task<Post> GetPostAsync(Guid id);
    Task<bool> CreatePostAsync(Post post);
    Task<bool> UpdatePostAsync(Guid id, Post post);
    Task<bool> RemovePostAsync(Guid id);
}
