using Microsoft.AspNetCore.Mvc;

namespace NetCafe.Server.Controllers;

public class NewsLetterSubsController : BaseController
{
    private readonly IMailingListRepository mailingListRepository;
    private readonly ILogger<NewsLetterSubsController> logger;

    public NewsLetterSubsController
    (
        IMailingListRepository mailingListRepository,
        ApplicationDbContext context,
        ILogger<NewsLetterSubsController> logger
    ) : base(context)
    {
        this.mailingListRepository = mailingListRepository;
        this.logger = logger;
    }

    [HttpPost("subscribe")]
    public async Task<IActionResult> Subscribe([FromBody] MailingListSubRequest model)
    {
        try
        {
            var sub = new NewsLetterSub()
            {
                Email = model.Email
            };
            var subscribeResult = await mailingListRepository.SubscribeAsync(sub);
            logger.LogInformation("New user subscribed to the mailing list with the email: {email}", model.Email);

            return Ok(new ApiResponse
            {
                Message = "You are now subscribed to the mailing list!",
                IsSuccess = true
            });
        }
        catch (DataInsertionFailedException ex)
        {
            logger.LogError("Failed to add the email: {email} to the mailing list", model.Email);
            return BadRequest(new ApiErrorResponse
            {
                Message = ex.Message
            });
        }
    }

    [HttpPost("unsubscribe")]
    public async Task<IActionResult> Unsubscribe([FromBody] MailingListSubRequest model)
    {
        try
        {
            var result = await mailingListRepository.UnsubscribeAsync(model.Email!);
            logger.LogInformation("User with email: {email} has been removed from the mailing list", model.Email);

            return Ok(new ApiResponse
            {
                Message = "You have unsubscribed from the mailing list",
                IsSuccess = true
            });
        }
        catch (NotFoundException ex)
        {
            logger.LogError("Failed to remove the email: {email} from the mailing list", model.Email);
            return BadRequest(new ApiErrorResponse
            {
                Message = ex.Message
            });
        }
    }
}
