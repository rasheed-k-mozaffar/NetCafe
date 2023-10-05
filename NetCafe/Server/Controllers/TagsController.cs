using Microsoft.AspNetCore.Mvc;
using NetCafe.Server.Controllers;

namespace NetCafe.Server;

public class TagsController : BaseController
{
    private readonly ITagsRepository tagsRepository;
    private readonly ILogger<TagsController> logger;

    public TagsController
    (
        ITagsRepository tagsRepository,
        ILogger<TagsController> logger,
        ApplicationDbContext context
    ) : base(context)
    {
        this.tagsRepository = tagsRepository;
        this.logger = logger;
    }

    #region  GET
    [HttpGet]
    public async Task<IActionResult> GetTags()
    {
        var tags = await tagsRepository.GetTagsAsync();
        var tagsAsDtos = tags.Select(t => t.ToTagDto());

        logger.LogInformation("All tags were retrieved successfully");
        return Ok(new ApiResponse<IEnumerable<TagDto>>
        {
            Message = "Tags retrieved successfully",
            Value = tagsAsDtos,
            IsSuccess = true
        });
    }

    [HttpGet("data/{id}")]
    public async Task<IActionResult> GetTagData(Guid id)
    {
        try
        {
            var tag = await tagsRepository.GetTagAsync(id);
            var tagDataAsDto = tag.ToTagData();

            logger.LogInformation("{tagName} with ID: {id} was successfully retrieved", tag.Name, id);
            return Ok(new ApiResponse<TagDataDto>
            {
                Message = $"{tag.Name} tag was retrieved successfully",
                Value = tagDataAsDto,
                IsSuccess = true
            });
        }
        catch (NotFoundException ex)
        {
            // not found tag
            logger.LogError("No tag was found with the ID: {id}", id);
            return BadRequest(new ApiErrorResponse
            {
                Message = ex.Message
            });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTag(Guid id)
    {
        try
        {
            var tag = await tagsRepository.GetTagAsync(id);
            var tagAsDto = tag.ToTagDto();

            logger.LogInformation("{tagName} with ID: {id} was successfully retrieved", tag.Name, id);
            return Ok(new ApiResponse<TagDto>
            {
                Message = $"{tag.Name} tag was retrieved successfully",
                Value = tagAsDto,
                IsSuccess = true
            });
        }
        catch (NotFoundException ex)
        {
            // not found tag
            logger.LogError("No tag was found with the ID: {id}", id);
            return BadRequest(new ApiErrorResponse
            {
                Message = ex.Message
            });
        }
    }
    #endregion
    #region POST
    [HttpPost]
    public async Task<IActionResult> CreateTag([FromBody] TagCreateDto model)
    {
        // valid data format
        if (ModelState.IsValid)
        {
            var tagToCreate = model.ToTagCreate();
            try
            {
                var creationResult = await tagsRepository.CreateTagAsync(tagToCreate);
                logger.LogInformation("{tagName} tag was created successfully", model.Name);
                return Ok(new ApiResponse
                {
                    Message = $"New {model.Name} tag was created successfully",
                    IsSuccess = true
                });
            }
            catch (DataInsertionFailedException ex)
            {
                // failed to create the new tag
                logger.LogError("Failed to create new tag");
                return BadRequest(new ApiErrorResponse
                {
                    Message = ex.Message,
                });
            }
        }
        else
        {
            return BadRequest(ModelState);
        }
    }
    #endregion
    #region DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTag(Guid id)
    {
        try
        {
            var result = await tagsRepository.RemoveTagAsync(id);
            logger.LogInformation("Tag with the ID: {id} was successfully removed", id);
            return Ok(new ApiResponse
            {
                Message = "Tag was removed successfully",
                IsSuccess = true
            });
        }
        catch (NotFoundException ex)
        {
            // no tag found to delete
            logger.LogError("No tag was found with the ID: {id}", id);
            return BadRequest(new ApiErrorResponse
            {
                Message = ex.Message
            });
        }
    }
    #endregion
    #region PUT
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTag(Guid id, [FromBody] TagUpdateDto model)
    {
        if (ModelState.IsValid)
        {
            var tagToUpdate = await context.Tags.FindAsync(id);
            // no tag found that matches the received ID
            if (tagToUpdate is null)
            {
                logger.LogError("No tag was found with the ID: {id}", id);
                return BadRequest(new ApiErrorResponse
                {
                    Message = "No tag was found with the given ID"
                });
            }
            // map the updated details
            tagToUpdate.Name = model.Name;
            tagToUpdate.Description = model.Description;

            await context.SaveChangesAsync();
            logger.LogInformation("Tag updated to {newTagName}", model.Name);
            return Ok(new ApiResponse
            {
                Message = "Tag details updated successfully",
                IsSuccess = true
            });
        }
        else
        {
            return BadRequest(ModelState);
        }
    }
    #endregion
}
