using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace NetCafe.Server.Controllers;
[Authorize(Roles = "USER,AUTHOR")]
public class CommentsController : BaseController
{
    private readonly ICommentsRepository commentsRepository;
    private readonly ILogger<CommentsController> logger;

    public CommentsController
    (
        ICommentsRepository commentsRepository,
        ILogger<CommentsController> logger,
        ApplicationDbContext context
    ) : base(context)
    {
        this.commentsRepository = commentsRepository;
        this.logger = logger;
    }

    #region GET
    [HttpGet("{postId}")]
    public async Task<IActionResult> GetCommentsForPost(Guid postId)
    {
        var comments = await commentsRepository.GetCommentsAsync(postId);
        if (!comments.Any())
        {
            // no comments
            logger.LogInformation("No comments for post with the ID: {id}", postId);
            return Ok(new ApiResponse
            {
                Message = "This post currently has no comments",
                IsSuccess = true
            });
        }

        // if there are comments, map them to DTOs
        var commentsAsDtos = comments.Select(c => c.ToCommentDto());
        logger.LogInformation("Commentrs for post with the ID: {id} were retrieved successfully", postId);
        return Ok(new ApiResponse<IEnumerable<CommentDto>>
        {
            Message = "Comments retrieved successfully",
            Value = commentsAsDtos,
            IsSuccess = true
        });
    }

    [HttpGet("comment/{id}")]
    public async Task<IActionResult> GetComment(Guid id)
    {
        try
        {
            var comment = await commentsRepository.GetCommentAsync(id);
            var commentAsDto = comment.ToCommentDto();

            logger.LogInformation("Comment with the ID: {id} was retrieved successfully", id);
            return Ok(new ApiResponse<CommentDto>
            {
                Message = "Comment retrieved successfully",
                Value = commentAsDto,
                IsSuccess = true
            });

        }
        catch (NotFoundException ex)
        {
            // didn't find the comment in the database
            logger.LogError("No comment was found with the ID: {id}", id);
            return BadRequest(new ApiErrorResponse
            {
                Message = ex.Message
            });
        }
    }
    #endregion
    #region POST
    [HttpPost("{postId}")]
    public async Task<IActionResult> AddCommentOnPost(Guid postId, [FromBody] CommentCreateDto model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var commentToAdd = model.ToCommentAdd();
                var creationResult = await commentsRepository.AddCommentAsync(postId, commentToAdd);

                logger.LogInformation("New comment was added to post with ID: {postId}", postId);
                return Ok(new ApiResponse
                {
                    Message = "New comment added successfully",
                    IsSuccess = true
                });
            }
            catch (DataInsertionFailedException ex)
            {
                logger.LogError("Failed to add new comment");
                return BadRequest(new ApiErrorResponse
                {
                    Message = ex.Message
                });
            }
        }
        else
        {
            logger.LogError("Invalid comment data format");
            return BadRequest(ModelState);
        }
    }

    [HttpPost("reply/{commentId}")]
    public async Task<IActionResult> ReplyToComment(Guid commentId, [FromBody] CommentCreateDto reply)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var replyToAdd = reply.ToCommentAdd();
                try
                {
                    var creationResult = await commentsRepository
                    .AddReplyToCommentAsync(commentId, replyToAdd);

                    logger.LogInformation("A new reply for comment with the ID: {id} was added successfully", commentId);
                    return Ok(new ApiResponse
                    {
                        Message = "Your reply has been added successfully",
                        IsSuccess = true
                    });
                }
                catch (NotFoundException ex)
                {
                    return BadRequest(new ApiErrorResponse
                    {
                        Message = ex.Message
                    });
                }
            }
            catch (DataInsertionFailedException ex)
            {
                logger.LogError("Failed to add new reply to comment with the ID: {id}", commentId);
                return BadRequest(new ApiErrorResponse
                {
                    Message = ex.Message
                });
            }
        }
        else
        {
            logger.LogError("Invalid reply comment data format");
            return BadRequest(ModelState);
        }
    }
    #endregion
    #region DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(Guid id)
    {
        try
        {
            var deletionResult = await commentsRepository.DeleteCommentAsync(id);

            if (deletionResult is true)
            {
                // deleting the comment was successful
                logger.LogInformation("Comment with the ID: {id} was successfully removed", id);
                return Ok(new ApiResponse
                {
                    Message = "Comment was successfully removed",
                    IsSuccess = true
                });
            }
            else
            {
                logger.LogError("Failed to remove comment with the ID: {id}", id);
                return BadRequest(new ApiErrorResponse
                {
                    Message = "Something went wrong while attempting to delete the comment"
                });
            }
        }
        catch (NotFoundException ex)
        {
            // didn't find the comment to delete
            logger.LogError("No comment was found with the ID: {id}", id);
            return BadRequest(new ApiErrorResponse
            {
                Message = ex.Message
            });
        }
    }
    #endregion
    #region PUT
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateComment(Guid id, [FromBody] CommentUpdateDto model)
    {
        if (ModelState.IsValid)
        {
            var commentToUpdate = await context.Comments.FindAsync(id);

            if (commentToUpdate is null)
            {
                // didn't find the comment to update
                logger.LogError("No comment was found with the ID: {id}", id);
                return BadRequest(new ApiErrorResponse
                {
                    Message = "No comment was found the given ID"
                });
            }
            // map the content
            commentToUpdate.Content = model.Content;
            await context.SaveChangesAsync();

            logger.LogInformation("Comment with the ID: {id} was updated successfully", id);
            return Ok(new ApiResponse
            {
                Message = "Your comment has been updated successfully",
                IsSuccess = true
            });
        }
        else
        {
            logger.LogError("Invalid comment data format");
            return BadRequest(ModelState);
        }
    }
    #endregion
}
