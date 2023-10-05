using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Internal;

namespace NetCafe.Client.Pages.Author;

public partial class TagsManager : ComponentBase
{
    [Inject]
    public ITagsService TagsService { get; set; } = default!;

    private List<TagSummaryDto> tags = new();
    private bool isLoadingData = true;
    private string errorMessage = string.Empty;

    // these variable are related to the confirm deletion dialog
    private bool wantsToDeleteTag = false;
    private Guid tagToDeleteId;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        // retrieve the tags data
        errorMessage = string.Empty;
        try
        {
            var results = await TagsService.GetAllTagsAsync();
            if (results.IsSuccess)
            {
                tags = results.Value!.ToList();
                isLoadingData = false;
            }
        }
        catch (DataRetrievalFailedException ex)
        {
            errorMessage = ex.Message;
        }
    }

    private async Task DeleteTagAsync()
    {
        errorMessage = string.Empty;
        try
        {
            var result = await TagsService.DeleteTagAsync(tagToDeleteId);
            if (result.IsSuccess)
            {
                // remove the tag from the table on the client without
                // making a request to the server
                var tagToDelete = tags.FirstOrDefault(t => t.TagId == tagToDeleteId);
                tags.Remove(tagToDelete!);
                wantsToDeleteTag = false;
                StateHasChanged();
            }
        }
        catch (DeletionFailedException ex)
        {
            errorMessage = ex.Message;
        }
    }

    private void CloseDialog()
    {
        tagToDeleteId = default;
        wantsToDeleteTag = false;
    }

    private void OpenDeleteDialog(Guid tagId)
    {
        wantsToDeleteTag = true;
        StateHasChanged();
        tagToDeleteId = tagId;
    }
}
