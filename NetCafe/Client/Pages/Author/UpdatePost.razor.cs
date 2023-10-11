using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace NetCafe.Client.Pages.Author;

public partial class UpdatePost : ComponentBase
{
    [Parameter] public Guid PostId { get; set; }

    #region Injected Dependencies
    [Inject]
    public NavigationManager Nav { get; set; } = default!;
    [Inject]
    public IPostsService PostsService { get; set; } = default!;
    [Inject]
    public ISeriesService SeriesService { get; set; } = default!;
    [Inject]
    public ITagsService TagsService { get; set; } = default!;
    [Inject]
    public IFilesService FilesService { get; set; } = default!;
    #endregion

    #region  Variables
    private bool isLoadingData = true;
    private bool isMakingRequest = false;
    private string errorMessage = string.Empty;
    private string successMessage = string.Empty;
    private PostDto oldPost = new();
    private PostUpdateDto updatedPost = new();

    private List<TagSummaryDto> tags = new();
    IEnumerable<Guid>? selectedTags;
    private string tagSelectionError = string.Empty;
    private List<SeriesSummaryDto> series = new();
    private Guid? selectedSeries;


    // File upload related properties
    private string[] allowedFileExtensions = { ".jpeg", ".png", ".jpg" };
    private int maxCoverImageSize = 1024 * 1024 * 5; // 5MB
    private string? fileUploadErrorMessage = string.Empty;
    #endregion

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        try
        {
            var result = await PostsService.GetPostByIdAsync(PostId);
            if (result.IsSuccess)
            {
                // map the retrieved post to a Post Update Object
                oldPost = result.Value!;
                updatedPost = new()
                {
                    Title = oldPost.Title,
                    Content = oldPost.Content,
                    CoverImageUrl = oldPost.CoverImageUrl
                };

                // map the tags
                if (oldPost.Tags is not null && oldPost.Tags.Any())
                {
                    selectedTags = oldPost.Tags.Select(t => t.TagId);
                }

                if (oldPost.SeriesId is not null)
                {
                    selectedSeries = oldPost.SeriesId;
                }
                isLoadingData = false;
            }
        }
        catch (DataRetrievalFailedException ex)
        {
            errorMessage = ex.Message;
        }
    }

    private void TagsChanged()
    {
        tagSelectionError = string.Empty;
        if (selectedTags is not null && selectedTags.Count() > 3)
        {
            selectedTags = Enumerable.Empty<Guid>();
            tagSelectionError = "You can't select more than 3 tags";
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        errorMessage = string.Empty;
        try
        {
            var tagResults = await TagsService.GetAllTagsAsync();
            if (tagResults.IsSuccess)
            {
                tags = tagResults.Value!.ToList();
            }

            var seriesResults = await SeriesService.GetAllSeriesAsync();
            if (seriesResults.IsSuccess)
            {
                series = seriesResults.Value!.ToList();
            }
        }
        catch (DataRetrievalFailedException ex)
        {
            errorMessage = ex.Message;
        }

    }

    private async Task HandlePostUpdateAsync()
    {
        isMakingRequest = true;
        errorMessage = string.Empty;
        try
        {
            if (selectedTags is not null && selectedTags.Any())
            {
                updatedPost.TagIds = selectedTags.ToArray();
            }

            if (selectedSeries is not null)
            {
                updatedPost.SeriesId = selectedSeries;
            }
            // change the publishing state to true so the post is public now
            updatedPost.IsPublished = true;
            var result = await PostsService.UpdatePostAsync(PostId, updatedPost);

            if (result.IsSuccess)
            {
                successMessage = "The post was successfully updated! you'll be redirected in a moment...";
                StateHasChanged();
                await Task.Delay(2000);
                Nav.NavigateTo($"/posts/{PostId}");
            }
        }
        catch (OperationFailureException ex)
        {
            errorMessage = ex.Message;
        }
        isMakingRequest = false;
    }

    private async Task SaveDraftAsync()
    {
        isMakingRequest = true;
        errorMessage = string.Empty;
        try
        {
            if (selectedTags is not null && selectedTags.Any())
            {
                updatedPost.TagIds = selectedTags.ToArray();
            }

            if (selectedSeries is not null)
            {
                updatedPost.SeriesId = selectedSeries;
            }
            // change the publishing state to false so the post is private now
            updatedPost.IsPublished = false;
            var result = await PostsService.UpdatePostAsync(PostId, updatedPost);

            if (result.IsSuccess)
            {
                successMessage = "The post was successfully updated! you'll be redirected in a moment...";
                StateHasChanged();
                await Task.Delay(2000);
                Nav.NavigateTo($"/posts/{PostId}");
            }
        }
        catch (OperationFailureException ex)
        {
            errorMessage = ex.Message;
        }
        isMakingRequest = false;
    }

    #region Cover Image Upload
    private async Task UploadCoverImage(InputFileChangeEventArgs e)
    {
        if (e.File is null)
        {
            fileUploadErrorMessage = "Please select a file!";
            return;
        }

        if (e.File.Size > maxCoverImageSize)
        {
            fileUploadErrorMessage = "The file you selected has a size that exceeded the max allowed size";
            return;
        }
        fileUploadErrorMessage = string.Empty;
        string extension = Path.GetExtension(e.File.Name);
        // use of an allowed extension
        if (!allowedFileExtensions.Contains(extension))
        {
            fileUploadErrorMessage = $"The image extension ({extension}) is not acceptable";
            return;
        }
        try
        {
            // convert IBrowseFile to an IFormFile 
            var imageFile = await ConvertToIFormFile(e.File);
            var result = await FilesService.UploadFileAsync(imageFile);
            // set the cover image URL to the retrieved image URL from the api
            updatedPost!.CoverImageUrl = result;
        }
        catch (FileUploadFailedException ex)
        {
            fileUploadErrorMessage = ex.Message;
        }
    }
    private async Task<Microsoft.AspNetCore.Http.IFormFile> ConvertToIFormFile(IBrowserFile browserFile)
    {
        Stream stream = browserFile.OpenReadStream(maxAllowedSize: maxCoverImageSize);
        MemoryStream memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        return new Microsoft.AspNetCore.Http.Internal.FormFile(memoryStream, 0, memoryStream.Length, "file", browserFile.Name);
    }
    #endregion
}
