namespace NetCafe.Server.Repositories;

public interface IImagesRepository
{
    Task<ICollection<Image>> GetAllImagesAsync();
    Task<ICollection<Image>> GetImagesForPostAsync(Guid postId);
    Task<Image> GetImageByIdAsync(Guid imageId);
    Task<bool> AddImageAsync(Image image);
    Task<bool> RemoveImageAsync(Guid imageId);
}