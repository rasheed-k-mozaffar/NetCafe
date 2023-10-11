using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace NetCafe.Client.Pages;

public partial class Post : ComponentBase
{
    [Parameter] public Guid PostId { get; set; }

    [Inject]
    public NavigationManager Nav { get; set; } = default!;
    [Inject]
    public IPostsService PostsService { get; set; } = default!;
    [Inject]
    public ISeriesService SeriesService { get; set; } = default!;

    private PostDto post = new();
    private SeriesDto? series;
    private bool isLoadingPost = true;
    private string errorMessage = string.Empty;

    private string postContent = string.Empty;

    private bool isLoadingSeriesData = true;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        errorMessage = string.Empty;
        try
        {
            var result = await PostsService.GetPostByIdAsync(PostId);
            if (result.IsSuccess)
            {
                // after loading the post, convert it into markup so that it can be displayed
                // in a valid format on the page
                post = result.Value!;
                postContent = Markdig.Markdown.ToHtml(post.Content!);
                isLoadingPost = false;
            }
        }
        catch (DataRetrievalFailedException ex)
        {
            errorMessage = ex.Message;
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (post.SeriesId is not null)
        {
            try
            {
                var seriesRetrievalResult = await SeriesService.GetSeriesByIdAsync((Guid)post.SeriesId);
                if (seriesRetrievalResult.IsSuccess)
                {
                    series = seriesRetrievalResult.Value;
                    isLoadingSeriesData = false;
                }
            }
            catch (DataRetrievalFailedException ex)
            {
                errorMessage = ex.Message;
            }
        }
    }
}
