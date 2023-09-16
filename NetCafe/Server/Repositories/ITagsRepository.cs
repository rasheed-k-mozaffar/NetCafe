namespace NetCafe.Server;

public interface ITagsRepository
{
    Task<ICollection<Tag>> GetTagsAsync();
    Task<Tag> GetTagAsync(Guid tagId);
    Task<bool> CreateTagAsync(Tag tag);
    Task<bool> RemoveTagAsync(Guid tagId);
    Task<bool> UpdateTagAsync();
}
