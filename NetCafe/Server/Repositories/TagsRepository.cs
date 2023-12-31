﻿
namespace NetCafe.Server.Repositories;

public class TagsRepository : ITagsRepository
{
    private readonly ApplicationDbContext context;

    public TagsRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<bool> CreateTagAsync(Tag tag)
    {
        bool alreadyExists = context.Tags.Any(t => t.Name == tag.Name);
        if (alreadyExists)
        {
            throw new DataInsertionFailedException(message: $"A tag with the name {tag.Name} already exists in the system");
        }

        var result = await context.Tags.AddAsync(tag);
        if (result.State == EntityState.Added)
        {
            await context.SaveChangesAsync();
            return true;
        }
        else
        {
            throw new DataInsertionFailedException(message: "Something went wrong while attempting to create the new tag.");
        }
    }

    public async Task<Tag> GetTagAsync(Guid tagId)
    {
        var tag = await context.Tags
            .FirstOrDefaultAsync(t => t.Id == tagId);

        if (tag is null)
        {
            throw new NotFoundException(message: "No tag was found with the given ID.");
        }

        return tag;
    }

    public async Task<ICollection<Tag>> GetTagsAsync()
    {
        var tags = await context.Tags
            .AsNoTracking().ToListAsync();

        return tags;
    }

    public async Task<bool> RemoveTagAsync(Guid tagId)
    {
        var tag = await context.Tags.FindAsync(tagId);

        if (tag is null)
        {
            throw new NotFoundException(message: "No tag was found with the given ID.");
        }

        // if there are posts linked with this tag, don't delete it
        if (tag.Posts is not null && tag.Posts.Any())
        {
            throw new RecordDeletionFailedException(message: "Couldn't delete comments as there are posts using it");
        }

        var result = context.Tags.Remove(tag);

        if (result.State == EntityState.Deleted)
        {
            await context.SaveChangesAsync();
            return true;
        }
        else
        {
            return false;
        }
    }

    public Task<bool> UpdateTagAsync()
    {
        throw new NotImplementedException();
    }
}
