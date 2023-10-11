using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Internal;

namespace NetCafe.Client.Pages.Author;

public partial class PostsManager : ComponentBase
{
    [Inject]
    public NavigationManager Nav { get; set; } = default!;
    [Inject]
    public IPostsService PostsService { get; set; } = default!;

    private List<PostSummaryDto> posts = new();
    private string errorMessage = string.Empty;
    private bool isLoadingData = true;

    // Delete posts related variables
    private bool wantsToDeletePost = false;
    private Guid postToDeleteId;
    private string deletionErrorMessage = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        errorMessage = string.Empty;
        try
        {
            var result = await PostsService.GetAllPostsAsync();
            if (result.IsSuccess)
            {
                posts = result.Value!.ToList();
                isLoadingData = false;
            }
        }
        catch (DataRetrievalFailedException ex)
        {
            errorMessage = ex.Message;
        }
    }

    private void OpenDeleteDialog(Guid postId)
    {
        wantsToDeletePost = true;
        postToDeleteId = postId;
    }

    private void CloseDialog()
    {
        wantsToDeletePost = false;
        postToDeleteId = default;
    }

    private async Task DeletePostAsync()
    {
        deletionErrorMessage = string.Empty;
        try
        {
            var result = await PostsService.DeletePostAsync(postToDeleteId);
            if (result.IsSuccess)
            {
                var postToDelete = posts.FirstOrDefault(p => p.PostId == postToDeleteId);
                posts.Remove(postToDelete!);
                wantsToDeletePost = false;
                postToDeleteId = default;
                StateHasChanged();
            }
        }
        catch (DeletionFailedException ex)
        {
            deletionErrorMessage = ex.Message;
            wantsToDeletePost = false;
            postToDeleteId = default;
        }
    }
}
