using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace NetCafe.Client.Pages.Author;

public partial class CreateSeries : ComponentBase
{
    [Inject]
    public NavigationManager Nav { get; set; } = default!;
    [Inject]
    public ISeriesService SeriesService { get; set; } = default!;
    [Inject]
    public IFilesService FilesService { get; set; } = default!;

    private bool isMakingRequest = false;
    private string errorMessage = string.Empty;
    private string successMessage = string.Empty;
    private SeriesCreateDto series = new();

    // File upload related properties
    private string[] allowedFileExtensions = { ".jpeg", ".png", ".jpg" };
    private int maxCoverImageSize = 1024 * 1024 * 5; // 5MB
    private string? fileUploadErrorMessage = string.Empty;

    private async Task HandleSeriesCreationAsync()
    {
        isMakingRequest = true;
        errorMessage = string.Empty;
        successMessage = string.Empty;
        try
        {
            var result = await SeriesService.CreateSeriesAsync(series);
            if (result.IsSuccess)
            {
                successMessage = $"New series ({series.Name}) was created successfully! You'll be redirected in a moment...";
                StateHasChanged();
                await Task.Delay(2000);
                Nav.NavigateTo("/author/dashboard");
            }
        }
        catch (DataInsertionFailedException ex)
        {
            errorMessage = ex.Message;
        }
        isMakingRequest = false;
    }

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
            var result = await FilesService.UploadFileAsync(imageFile);
            // set the cover image URL to the retrieved image URL from the api
            series.CoverImageUrl = result.Value;
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
}
