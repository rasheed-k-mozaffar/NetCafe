namespace NetCafe.Server.Repositories;

public interface IMailingListRepository
{
    Task<bool> SubscribeAsync(NewsLetterSub sub);
    Task<bool> UnsubscribeAsync(string email);
}
