
namespace NetCafe.Server.Repositories;

public class PostsRepository : IPostsRepository
{
    private readonly ApplicationDbContext context;

    public PostsRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<bool> CreatePostAsync(Post post)
    {
        var result = await context.Posts.AddAsync(post);
        if (result.State == EntityState.Added)
        {
            await context.SaveChangesAsync();
            // if the post is added successfully, return true
            return true;
        }
        else
        {
            throw new DataInsertionFailedException(message: "Something went wrong while attempting to add the new post.");
        }
    }

    public async Task<Post> GetPostAsync(Guid id)
    {
        // FirstOfDefault doesn't require the entity to 
        // be tracked by the context
        var post = await context.Posts
            .FirstOrDefaultAsync(p => p.Id == id);

        if (post is null)
        {
            throw new NotFoundException(message: "No post was found with the given ID");
        }
        else
        {
            // The post was found, return it
            return post;
        }
    }

    public async Task<ICollection<Post>> GetPostsAsync()
    {
        var posts = await context.Posts
            .ToListAsync();

        if (posts is null)
        {
            // There are no posts in the database
            return new List<Post>();
        }
        else
        {
            return posts;
        }
    }

    public async Task<bool> RemovePostAsync(Guid id)
    {
        var postToDelete = await context.Posts.FindAsync(id);

        if (postToDelete is null)
        {
            throw new NotFoundException(message: "No post was found with the given ID");
        }
        else
        {
            var result = context.Posts.Remove(postToDelete);
            if (result.State == EntityState.Deleted)
            {
                await context.SaveChangesAsync();
                // The post was deleted successfully.
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public Task<bool> UpdatePostAsync(Guid id, Post post)
    {
        throw new NotImplementedException();
    }
}
