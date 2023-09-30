namespace NetCafe.Client.Services;

public interface IMailingListService
{
    Task<ApiResponse> SubscribeAsync(MailingListSubRequest request);
    Task<ApiResponse> UnsubscribeAsync(MailingListSubRequest request);
}
