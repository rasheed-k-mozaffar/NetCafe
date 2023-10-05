using Microsoft.AspNetCore.Components;

namespace NetCafe.Client.Pages.Author;

public partial class CreateTag : ComponentBase
{
    [Inject]
    public NavigationManager Nav { get; set; } = default!;
    [Inject]
    public ITagsService TagsService { get; set; } = default!;

    private TagCreateDto tag = new();
    private bool isMakingRequest = false;
    private string? errorMessage = string.Empty;
    private string? successMessage = string.Empty;

    private async Task HandleTagCreationAsync()
    {
        isMakingRequest = true;
        errorMessage = string.Empty;
        successMessage = string.Empty;
        try
        {
            var result = await TagsService.CreateTagAsync(tag);
            if (result.IsSuccess)
            {
                successMessage = $"New tag ({tag.Name}) was created successfully! You'll be redirected in a moment...";
                StateHasChanged();
                await Task.Delay(3000); // wait three seconds before redirecting
                Nav.NavigateTo("/author/dashboard");
            }
        }
        catch (DataInsertionFailedException ex)
        {
            // error message
            errorMessage = ex.Message;
        }
        isMakingRequest = false;
    }
}

