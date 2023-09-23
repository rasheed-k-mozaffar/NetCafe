
using Microsoft.AspNetCore.Http.HttpResults;

namespace NetCafe.Server.Repositories;

public class ImagesRepository : IImagesRepository
{
    private readonly ApplicationDbContext context;

    public ImagesRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<bool> AddImageAsync(Image image)
    {
        var result = await context.Images.AddAsync(image);

        if (result.State == EntityState.Added)
        {
            await context.SaveChangesAsync();
            return true;
        }
        else
        {
            throw new DataInsertionFailedException(message: "Something went wrong while attemptint to add the new image");
        }
    }

    public async Task<Image> GetImageByIdAsync(Guid imageId)
    {
        var image = await context.Images
        .FirstOrDefaultAsync(i => i.Id == imageId);

        if (image is null)
        {
            throw new NotFoundException(message: "No image was found with the given ID");
        }

        return image;
    }

    public async Task<ICollection<Image>> GetImagesForPostAsync(Guid postId)
    {
        var images = await context.Images
        .Where(i => i.PostId == postId).ToListAsync();

        return images;
    }

    public async Task<ICollection<Image>> GetAllImagesAsync()
    {
        var images = await context.Images.AsNoTracking().ToListAsync();
        return images;
    }

    public async Task<bool> RemoveImageAsync(Guid imageId)
    {
        var imageToDelete = await context.Images.FindAsync(imageId);

        if (imageToDelete is null)
        {
            throw new NotFoundException(message: "No image was found with the given ID");
        }

        var result = context.Images.Remove(imageToDelete);

        if (result.State == EntityState.Deleted)
        {
            await context.SaveChangesAsync();
            return true;
        }

        return false;
    }
}
