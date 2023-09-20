using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Mvc;
using NetCafe.Shared;

namespace NetCafe.Server.Controllers;

public class PostsController : BaseController
{
    private static string[] allowedImageExtensions = { ".jpg", ".jpeg", ".png" };
    private readonly IPostsRepository postsRepository;
    private readonly ILogger<PostsController> logger;
    public PostsController
    (
        IPostsRepository postsRepository,
        ILogger<PostsController> logger,
        ApplicationDbContext context
    ) : base(context)
    {
        this.postsRepository = postsRepository;
        this.logger = logger;
    }

    #region GET
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var posts = await postsRepository.GetPostsAsync();

        if (posts is null || !posts.Any())
        {
            logger.LogInformation("No posts were found in the database to retrieve");
            return Ok(new ApiResponse
            {
                Message = "There are currently no posts to view",
                IsSuccess = true
            });
        }

        var postsAsDtos = posts.Select(p => p.ToPostSummary());
        logger.LogInformation("All posts were retrieved successfully");
        return Ok(new ApiResponse<IEnumerable<PostSummaryDto>>
        {
            Message = "Posts retrieved successfully",
            Value = postsAsDtos,
            IsSuccess = true
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPost(Guid id)
    {
        try
        {
            var post = await postsRepository.GetPostAsync(id);
            var postAsDto = post.ToPost(); // map the post

            logger.LogInformation("Post with ID {id} was successfully retrieved", id);
            return Ok(new ApiResponse<PostDto>
            {
                Message = "Post retrieved successfully",
                Value = postAsDto,
                IsSuccess = true
            });
        }
        catch (NotFoundException ex)
        {
            logger.LogError("Couldn't find a post with the ID: {id}", id);
            // no post in the database has the given id
            return BadRequest(new ApiErrorResponse
            {
                Message = ex.Message
            });
        }
    }
    #endregion
    #region POST
    [HttpPost]
    public async Task<IActionResult> CreatePost(PostCreateDto model)
    {
        if (ModelState.IsValid)
        {
            byte[]? image = null;
            // handle the image file extension, if the extension isn't allowed, refuse the creation
            // of the post and return a bad request with a messsage telling the user to use a viable file extension
            if (model.CoverImage is not null)
            {
                string imageExtension = Path.GetExtension(model.CoverImage.FileName);
                // allowed image extension
                if (allowedImageExtensions.Contains(imageExtension))
                {
                    // convert the image from a file, to a byte array to store in the db
                    image = await Helpers.ConvertFileToByteArrayAsync(model.CoverImage);
                }
                else
                {
                    logger.LogError("Failed to publish {title} due to invalid cover image file extension", model.Title);
                    // use of unallowed image extension
                    return BadRequest(new ApiErrorResponse
                    {
                        Message = $"The file extension ({imageExtension}) is not allowed"
                    });
                }
            }
            // map the incoming data
            var postToCreate = model.ToPostCreate();
            postToCreate.CoverImage = image;

            // map the tags
            if (model.TagIds is not null && model.TagIds.Any())
            {
                foreach (var tagId in model.TagIds)
                {
                    var tag = await context.Tags.FindAsync(tagId);

                    if (tag is not null)
                    {
                        postToCreate.Tags?.Add(tag);
                    }
                }
            }

            try
            {
                var creationResult = await postsRepository
                .CreatePostAsync(post: postToCreate);
                logger.LogInformation("New post {title} was published successfully", model.Title);
                // successful post creation
                return Ok(new ApiResponse
                {
                    Message = "Post published successfully",
                    IsSuccess = true
                });
            }
            catch (DataInsertionFailedException ex)
            {
                logger.LogError("Failed to publish new post due to a data insertion failure");
                // post creation failure
                return BadRequest(new ApiErrorResponse
                {
                    Message = ex.Message
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
    public async Task<IActionResult> DeletePost(Guid id)
    {
        try
        {
            var deletionResult = await postsRepository.RemovePostAsync(id);
            if (deletionResult is true)
            {
                logger.LogInformation("Post with ID {id} was successfully deleted", id);
                return Ok(new ApiResponse
                {
                    Message = "Post deleted successfully",
                    IsSuccess = true
                });
            }
            else
            {
                // deletion failure
                logger.LogError("Failed to delete the post with ID: {id}", id);
                return BadRequest(new ApiErrorResponse
                {
                    Message = "Deleting the post failed"
                });
            }
        }
        catch (NotFoundException ex)
        {
            // post not found
            logger.LogError("No post was found with ID: {id}", id);
            return BadRequest(new ApiErrorResponse
            {
                Message = ex.Message
            });
        }
    }
    #endregion
    #region PUT
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePost(Guid id, PostUpdateDto model)
    {
        if (ModelState.IsValid)
        {
            byte[]? image = null;
            if (model.CoverImage is not null)
            {
                string imageExtension = Path.GetExtension(model.CoverImage.FileName);
                if (allowedImageExtensions.Contains(imageExtension))
                {
                    image = await Helpers.ConvertFileToByteArrayAsync(model.CoverImage);
                }
                else
                {
                    logger.LogError("Failed to edit {title} due to invalid cover image file extension", model.Title);
                    // use of unallowed image extension
                    return BadRequest(new ApiErrorResponse
                    {
                        Message = $"The file extension ({imageExtension}) is not allowed"
                    });
                }
            }

            var postToUpdate = await context.Posts.FindAsync(id);
            if (postToUpdate is null)
            {
                // post not found
                logger.LogError("No post was found with the ID: {id}", id);
                return BadRequest(new ApiErrorResponse
                {
                    Message = "No post was found with the given ID"
                });
            }

            // update the post's properties with the new values
            postToUpdate.CoverImage = image;
            postToUpdate.Title = model.Title;
            postToUpdate.Content = model.Content;
            postToUpdate.ModifiedOn = DateTime.UtcNow;
            postToUpdate.SeriesId = model.SeriesId;
            postToUpdate.Tags = model.Tags?.Select(t => t.ToTag()).ToList();

            await context.SaveChangesAsync();
            logger.LogInformation("Post with ID: {id} was successfully updated", id);
            return Ok(new ApiResponse
            {
                Message = "Post was successfully updated",
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
