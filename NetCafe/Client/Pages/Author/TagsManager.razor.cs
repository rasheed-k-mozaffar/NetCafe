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

    // these variables are related to the action dialog
    // in case there's an attempt to update a tag, initialize this instance
    private ApiResponse<TagDto>? tagRetrievalResult;
    private TagUpdateDto? updatedTag; // this object will be sent to update the tag
    private bool wantsToUpdateTag = false;
    private bool isRetrievingTagDetails = false;
    private bool makingUpdateRequest = false;
    private string tagUpdateErrorMessage = string.Empty;
    private Guid tagToUpdateId;

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
        tagToDeleteId = tagId;
    }

    private void CloseActionDialog()
    {
        wantsToUpdateTag = false;
        // reset the objects
        tagRetrievalResult = null;
        updatedTag = null;
    }

    private async Task OpenActionDialog(Guid tagId)
    {
        tagToUpdateId = tagId;
        isRetrievingTagDetails = true;
        tagUpdateErrorMessage = string.Empty;
        wantsToUpdateTag = true;

        // retrieve the tag details from the database
        try
        {
            tagRetrievalResult = await TagsService.GetTagByIdAsync(tagToUpdateId);
            // tag was successfully retrieved
            if (tagRetrievalResult.IsSuccess)
            {
                // map the details to the object used in the EditForm
                updatedTag = new()
                {
                    Name = tagRetrievalResult.Value!.Name,
                    Description = tagRetrievalResult.Value!.Description
                };

                // tag is ready to be displayed
                isRetrievingTagDetails = false;
            }
        }
        catch (DataRetrievalFailedException ex)
        {
            tagUpdateErrorMessage = ex.Message;
        }
    }

    private async Task UpdateTagAsync()
    {
        makingUpdateRequest = true;
        tagUpdateErrorMessage = string.Empty;
        try
        {
            var result = await TagsService.UpdateTagAsync(tagToUpdateId, updatedTag!);
            if (result.IsSuccess)
            {
                // update the tag on the client
                var tagToUpdate = tags.FirstOrDefault(t => t.TagId == tagToUpdateId);
                if (tagToUpdate is not null)
                {
                    tagToUpdate.Name = updatedTag!.Name;
                }
            }
        }
        catch (OperationFailureException ex)
        {
            tagUpdateErrorMessage = ex.Message;
        }
        // reset the variables involved in updating the tag
        makingUpdateRequest = false;
        wantsToUpdateTag = false;
        tagRetrievalResult = null;
        updatedTag = null;
        tagToUpdateId = default;
    }
}
