
namespace NetCafe.Server.Repositories;

public class MailingListRepository : IMailingListRepository
{
    private readonly ApplicationDbContext context;

    public MailingListRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<bool> SubscribeAsync(NewsLetterSub sub)
    {
        // check to see if the user is already subscribed to the news letter
        bool alreadySubscibred = await context.NewsLetterSubs.AnyAsync(s => s.Email == sub.Email);

        if (alreadySubscibred)
        {
            throw new DataInsertionFailedException(message: "You're already subscribed to the mailing list");
        }

        var result = await context.NewsLetterSubs.AddAsync(sub);

        if (result.State == EntityState.Added)
        {
            await context.SaveChangesAsync();
            return true;
        }
        else
        {
            throw new DataInsertionFailedException("New email couldn't be signed up for the mailing list");
        }
    }

    public async Task<bool> UnsubscribeAsync(string email)
    {
        var sub = await context.NewsLetterSubs
        .SingleOrDefaultAsync(s => s.Email == email);

        if (sub is null)
        {
            throw new NotFoundException(message: "The subscriber to remove doesn't exist");
        }
        var result = context.Remove(sub);

        if (result.State == EntityState.Deleted)
        {
            await context.SaveChangesAsync();
            return true;
        }

        // the removal just failed
        return false;
    }
}
