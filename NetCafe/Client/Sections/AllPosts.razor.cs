using Microsoft.AspNetCore.Components;

namespace NetCafe.Client.Sections;

public partial class AllPosts : ComponentBase
{
    [Inject]
    public NavigationManager Navigation { get; set; } = default!;
    [Inject]
    public IPostsService PostsService { get; set; } = default!;


    private ApiResponse<IEnumerable<PostSummaryDto>>? requestResult;
    private List<PostSummaryDto>? posts;
    private bool isLoadingPosts = true;
    private string errorMessage = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        isLoadingPosts = true;
        errorMessage = string.Empty;
        try
        {
            requestResult = await PostsService.GetAllPostsAsync();
            if (requestResult.IsSuccess)
            {
                posts = requestResult.Value?.ToList();
            }
        }
        catch (DataRetrievalFailedException ex)
        {
            errorMessage = ex.Message;
        }
        isLoadingPosts = false;
    }
}
