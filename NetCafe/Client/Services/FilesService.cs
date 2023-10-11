using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace NetCafe.Client.Services;

public class FilesService : IFilesService
{
    private readonly HttpClient httpClient;

    public FilesService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        var formData = new MultipartFormDataContent();
        var streamContent = new StreamContent(file.OpenReadStream());
        formData.Add(streamContent, "file", file.FileName);
        var response = await httpClient.PostAsync("/api/files", formData);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new FileUploadFailedException(message: error!.Message!);
        }
        var result = await response.Content.ReadFromJsonAsync<UrlObject>();
        return result!.Url!;
    }
}

public class UrlObject {
    public string? Url { get; set; }
}

