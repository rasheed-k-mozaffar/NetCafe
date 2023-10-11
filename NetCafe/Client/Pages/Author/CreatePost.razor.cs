using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;

namespace NetCafe.Client.Pages.Author;

public partial class CreatePost : ComponentBase
{
    #region Injected Dependencies
    [Inject]
    public NavigationManager Navigation { get; set; } = default!;
    [Inject]
    public IFilesService FilesService { get; set; } = default!;
    [Inject]
    public IPostsService PostsService { get; set; } = default!;
    [Inject]
    public ITagsService TagsService { get; set; } = default!;
    [Inject]
    public ISeriesService SeriesService { get; set; } = default!;
    #endregion

    #region variables
    private bool isMakingRequest = false;
    private string errorMessage = string.Empty;
    private PostCreateDto post = new();

    // Series select related variables
    private List<SeriesSummaryDto> series = new();
    private Guid? selectedSeries;

    // Tag select related variables
    private List<TagSummaryDto> tags = new();
    IEnumerable<Guid>? selectedTags;
    private string tagSelectionError = string.Empty;

    // File upload related properties
    private string[] allowedFileExtensions = { ".jpeg", ".png", ".jpg" };
    private int maxCoverImageSize = 1024 * 1024 * 5; // 5MB
    private string? fileUploadErrorMessage = string.Empty;
    #endregion

    protected override async Task OnInitializedAsync()
    {
        errorMessage = string.Empty;
        await base.OnInitializedAsync();
        try
        {
            var result = await TagsService.GetAllTagsAsync();
            if (result.IsSuccess)
            {
                tags = result.Value!.ToList();
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

    private void TagsChanged()
    {
        tagSelectionError = string.Empty;
        if (selectedTags is not null && selectedTags.Count() > 3)
        {
            selectedTags = Enumerable.Empty<Guid>();
            tagSelectionError = "You can't select more than 3 tags";
        }
    }
    private async Task HandlePostCreation()
    {
        isMakingRequest = true;
        errorMessage = string.Empty;
        try
        {
            if (selectedTags is not null && selectedTags.Any())
            {
                post.TagIds = selectedTags.ToArray();
            }

            if (selectedSeries is not null)
            {
                post.SeriesId = selectedSeries;
            }
            var result = await PostsService.CreatePostAsync(post);
            if (result.IsSuccess)
            {
                Console.WriteLine(result.Message);
            }
        }
        catch (DataInsertionFailedException ex)
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
        isMakingRequest = true;
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
            string result = await FilesService.UploadFileAsync(imageFile);
            // set the cover image URL to the retrieved image URL from the api
            post.CoverImageUrl = result;
        }
        catch (FileUploadFailedException ex)
        {
            fileUploadErrorMessage = ex.Message;
        }
        isMakingRequest = false;
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
