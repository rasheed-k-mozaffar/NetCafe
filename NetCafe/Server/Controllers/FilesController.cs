using System.Windows.Markup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace NetCafe.Server.Controllers;

public class FilesController : BaseController
{
    private static string[] allowedImageExtensions = { ".jpg", ".jpeg", ".png" };
    private readonly ILogger<FilesController> logger;

    public FilesController
    (
        ILogger<FilesController> logger,
        ApplicationDbContext context
    ) : base(context)
    {
        this.logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] IFormFile file)
    {
        if (file is null)
        {
            logger.LogError("Failed to upload a new file as no file was supplied");
            return BadRequest(new ApiErrorResponse
            {
                Message = "The file is required"
            });
        }

        string fileName = file.FileName;
        string extension = Path.GetExtension(fileName);

        // check if the file extension is allowed or not
        if (!allowedImageExtensions.Contains(extension))
        {
            logger.LogError("Failed to upload new file (File has invalid extension)");
            return BadRequest(new ApiErrorResponse
            {
                Message = $"The file extension ({extension}) is not allowed"
            });
        }

        // Create a new file name to avoid duplication between images.
        string newFileName = $"{Path.GetFileNameWithoutExtension(fileName)}-{Guid.NewGuid().ToString()}{extension}";
        // Get the path to which the images will be sent
        string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
        // Create the path for the file, combining the path of the application, with the file name
        string filePath = Path.Combine(directoryPath, newFileName);

        // Ensure the files directory is created, if not, create it
        Directory.CreateDirectory(directoryPath);

        using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            // Copy the content to the new file stream
            await file.CopyToAsync(stream);
        }

        logger.LogInformation("New iamge was uploaded: File Name: {fileName}", newFileName);
        return Ok(new ApiResponse<string>
        {
            Message = "File uploaded successfully",
            Value = $"http://localhost:5171/Images/{newFileName}",
            IsSuccess = true
        });
    }
}