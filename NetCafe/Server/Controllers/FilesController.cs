using System.Windows.Markup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace NetCafe.Server.Controllers;

public class FilesController : BaseController
{
    private readonly IWebHostEnvironment environment;
    private readonly ILogger<FilesController> logger;

    public FilesController
    (
        ILogger<FilesController> logger,
        ApplicationDbContext context
,
        IWebHostEnvironment environment) : base(context)
    {
        this.logger = logger;
        this.environment = environment;
    }

    [HttpPost]
    public IActionResult Upload(IFormFile file)
    {
        try
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            using (var stream = new FileStream(Path.Combine(environment.WebRootPath, fileName), FileMode.Create))
            {
                // Save the file
                file.CopyTo(stream);
                // Return the URL of the file
                var url = Url.Content($"~/{fileName}");
                return Ok(new {Url = url});
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}