using Microsoft.AspNetCore.Http;

namespace NetCafe.Client.Services;

public interface IFilesService
{
    Task<ApiResponse<string>> UploadFileAsync(IFormFile file);
}
