using Microsoft.AspNetCore.Components;

namespace NetCafe.Client.Sections;

public partial class AllPosts : ComponentBase
{
    [Inject]
    public NavigationManager Navigation { get; set; } = default!;
    [Inject]
    public IPostsService PostsService { get; set; } = default!;

    // these two parameters are concered with search and filtering
    private string? searchText;

    private ApiResponse<IEnumerable<PostSummaryDto>>? requestResult;
    private List<PostSummaryDto>? posts;
    private List<PostSummaryDto>? filteredPosts;
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
                // only display the posts that are publicly available 
                // (is published is true)
                posts = requestResult.Value?
                .Where(p => p.IsPublished == true)
                .ToList();
            }
        }
        catch (DataRetrievalFailedException ex)
        {
            errorMessage = ex.Message;
        }
        isLoadingPosts = false;
    }

    private void FilterPosts(ChangeEventArgs e)
    {
        searchText = e.Value!.ToString();
        filteredPosts = posts!.Where(p => p.Title!.ToLower().Contains(searchText.ToLower())).ToList();
    }
}
