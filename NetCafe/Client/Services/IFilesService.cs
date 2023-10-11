using Microsoft.AspNetCore.Http;

namespace NetCafe.Client.Services;

public interface IFilesService
{
    Task<string> UploadFileAsync(IFormFile file);
}
