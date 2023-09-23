using Microsoft.AspNetCore.Mvc;

namespace NetCafe.Server.Controllers;

public class FilesController : BaseController
{
    private static string[] allowedImageExtensions = { ".jpg", ".jpeg", ".png" };
    private readonly IImagesRepository imagesRepository;
    private readonly ILogger<FilesController> logger;

    public FilesController
    (
        IImagesRepository imagesRepository,
        ILogger<FilesController> logger,
        ApplicationDbContext context
    ) : base(context)
    {
        this.imagesRepository = imagesRepository;
        this.logger = logger;
    }
    #region POST
    [HttpPost("upload-post-image/{postId}")]
    public async Task<IActionResult> UploadPostImage(Guid postId, [FromForm] IFormFile file)
    {
        if (file is null)
        {
            return BadRequest("File is required");
        }

        // grab the file name and extension
        string fileName = file.FileName;
        string fileExtension = Path.GetExtension(fileName);

        if (!allowedImageExtensions.Contains(fileExtension))
        {
            logger.LogError("Failed to add new image due to the use of unacceptable file extension ({extension})", fileExtension);
            // use of unaccepted file extension
            return BadRequest(new ApiErrorResponse
            {
                Message = $"The file extension ({fileExtension}) is not acceptable"
            });
        }

        // create a new file to make sure duplicates won't exist in the images folder
        string newFileName = $"{Path.GetFileNameWithoutExtension(fileName)}-{Guid.NewGuid().ToString()}{fileExtension}";

        // create the path of the folder in which the images will be stored
        string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
        string fullPath = Path.Combine(directoryPath, newFileName);

        // create the directory if doesn't exist
        Directory.CreateDirectory(directoryPath);

        using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
        {
            // copy the content to the new stream
            await file.CopyToAsync(fileStream);
        }

        try
        {
            Image image = new()
            {
                PostId = postId,
                SeriesId = null,
                FileName = newFileName,
                URL = $"http://localhost:5171/Images/{newFileName}"
            };
            var creationResult = await imagesRepository.AddImageAsync(image);

            if (creationResult is false)
            {
                logger.LogError("Couldn't add new image to the database");
                return BadRequest(new ApiErrorResponse
                {
                    Message = "Something went wrong, please try again"
                });
            }
        }
        catch (DataInsertionFailedException ex)
        {
            // the insertion of the new image file failed
            return BadRequest(new ApiErrorResponse
            {
                Message = ex.Message
            });
        }

        logger.LogInformation("New image was added successfully");
        // return the URL of the image
        return Ok(new ApiResponse<string>
        {
            Message = "New image was added successfully",
            Value = $"http://localhost:5171/Images/{newFileName}",
            IsSuccess = true
        });
    }

    [HttpPost("upload-series-image/{seriesId}")]
    public async Task<IActionResult> UploadSeriesImage(Guid seriesId, [FromForm] IFormFile file)
    {
        if (file is null)
        {
            return BadRequest("File is required");
        }

        var fileName = file.FileName;
        var fileExtension = Path.GetExtension(fileName);

        if (!allowedImageExtensions.Contains(fileExtension))
        {
            logger.LogError("Failed to add new image due to the use of unacceptable file extension ({extension})", fileExtension);
            // use of unaccepted file extension
            return BadRequest(new ApiErrorResponse
            {
                Message = $"The file extension ({fileExtension}) is not acceptable"
            });
        }

        var newFileName = $"{Path.Combine(Path.GetFileNameWithoutExtension(fileName))}-{Guid.NewGuid().ToString()}{fileExtension}";

        var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
        var fullPath = Path.Combine(directoryPath, newFileName);

        Directory.CreateDirectory(directoryPath);

        using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
        {
            // copy the content to the new stream
            await file.CopyToAsync(fileStream);
        }

        try
        {
            Image image = new()
            {
                PostId = null,
                SeriesId = seriesId,
                FileName = newFileName,
                URL = $"http://localhost:5171/Images/{newFileName}"
            };
            var creationResult = await imagesRepository.AddImageAsync(image);

            if (creationResult is false)
            {
                logger.LogError("Couldn't add new image to the database");
                return BadRequest(new ApiErrorResponse
                {
                    Message = "Something went wrong, please try again"
                });
            }
        }
        catch (DataInsertionFailedException ex)
        {
            // the insertion of the new image file failed
            return BadRequest(new ApiErrorResponse
            {
                Message = ex.Message
            });
        }

        logger.LogInformation("New image was added successfully");
        // return the URL of the image
        return Ok(new ApiResponse<string>
        {
            Message = "New image was added successfully",
            Value = $"http://localhost:5171/Images/{newFileName}",
            IsSuccess = true
        });
    }
    #endregion
    #region GET
    [HttpGet("images")]
    public async Task<IActionResult> GetAllImages()
    {
        var images = await imagesRepository.GetAllImagesAsync();
        var imagesAsDtos = images.Select(i => i.ToImageDto());

        logger.LogInformation("All images have been retrieved successfully");
        return Ok(new ApiResponse<IEnumerable<ImageDto>>
        {
            Message = "Images retrieved successfully",
            Value = imagesAsDtos,
            IsSuccess = true
        });
    }
    #endregion
}