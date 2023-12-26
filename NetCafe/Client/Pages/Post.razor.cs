using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Remotion.Linq.Clauses;

namespace NetCafe.Client.Pages;

public partial class Post : ComponentBase
{
    [Parameter] public Guid PostId { get; set; }

    [Inject]
    public NavigationManager Nav { get; set; } = default!;
    [Inject]
    public IPostsService PostsService { get; set; } = default!;
    [Inject]
    public ICommentsService CommentsService { get; set; } = default!;
    [Inject]
    public ISeriesService SeriesService { get; set; } = default!;

    private PostDto post = new();
    private SeriesDto? series;
    private List<CommentDto>? comments;
    private CommentCreateDto? commentCreate;
    private CommentUpdateDto? updatedComment;
    private bool wantsToAddComment = false;
    private bool wantsToDeleteComment = false;
    private bool wantsToUpdateComment = false;
    private bool isMakingUpdateRequest = false;
    private Guid commentToDeleteId;
    private Guid commentToUpdateId;
    // private bool wantsToAddReply = false;
    private bool isLoadingPost = true;
    private bool isLoadingComments = true;
    private bool isMakingRequest = false;
    private string errorMessage = string.Empty;

    private string postContent = string.Empty;

    private bool isLoadingSeriesData = true;

    protected override async Task OnInitializedAsync() {
        await base.OnInitializedAsync();
        errorMessage = string.Empty;
        try {
            var result = await PostsService.GetPostByIdAsync(PostId);
            if (result.IsSuccess) {
                // after loading the post, convert it into markup so that it can be displayed
                // in a valid format on the page
                post = result.Value!;
                postContent = Markdig.Markdown.ToHtml(post.Content!);
                isLoadingPost = false;
                if (post.SeriesId.HasValue) {
                    try {
                        var seriesRetrievalResult = await SeriesService.GetSeriesByIdAsync((Guid)post.SeriesId);
                        if (seriesRetrievalResult.IsSuccess) {
                            series = seriesRetrievalResult.Value;
                            isLoadingSeriesData = false;
                        }
                    }
                    catch (DataRetrievalFailedException ex) {
                        errorMessage = ex.Message;
                    }
                }
            }
        }
        catch (DataRetrievalFailedException ex) {
            errorMessage = ex.Message;
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        await base.OnAfterRenderAsync(firstRender);
        // Load the comments section
        isLoadingComments = true;
        try {
            var result = await CommentsService.GetCommentsForPostAsync(PostId);
            if (result.IsSuccess) {
                if (result.Value is not null) {
                    comments = result.Value.ToList();
                }
                else {
                    comments = new();
                }
                isLoadingComments = false;
            }
        }
        catch (DataRetrievalFailedException ex) {
            errorMessage = ex.Message;
        }

    }

    private async Task AddCommentAsync() {
        errorMessage = string.Empty;
        isMakingRequest = true;
        try {
            var result = await CommentsService.AddCommentAsync(PostId, commentCreate!);
            if (result.IsSuccess) {
                // add the new comment to the list of comments on the client
                wantsToAddComment = false;
                commentCreate = null;
                comments!.Add(result.Value!);
                isMakingRequest = false;
            }
        }
        catch (DataInsertionFailedException ex) {
            errorMessage = ex.Message;
        }
        isMakingRequest = false;
    }

    private void ShowAddCommentForm() {
        commentCreate = new();
        wantsToAddComment = true;
    }

    private void CancelComment() {
        commentCreate = null;
        wantsToAddComment = false;
    }

    private void OpenDeleteCommentDialog(Guid commentId) {
        wantsToDeleteComment = true;
        commentToDeleteId = commentId;
    }

    private void CloseCommentDeleteDialog() {
        wantsToDeleteComment = false;
        commentToDeleteId = default;
    }

    private async Task DeleteCommentAsync() {
        try {
            var result = await CommentsService.DeleteCommentAsync(commentToDeleteId);
            if (result.IsSuccess) {
                // delete the comment from the client-side list wihtout making a request
                var commentToDelete = comments!.FirstOrDefault(c => c.CommentId == commentToDeleteId);
                if (commentToDelete is not null) {
                    comments!.Remove(commentToDelete);
                    wantsToDeleteComment = false;
                    commentToDeleteId = default;
                }
            }
        }
        catch (DeletionFailedException ex) {
            errorMessage = ex.Message;
        }
    }

    private void ShowEditCommentDialog(Guid commentId, string commentContent) {
        wantsToUpdateComment = true;
        commentToUpdateId = commentId;
        updatedComment = new() { Content = commentContent };
    }

    private void CloseActionDialog() {
        wantsToUpdateComment = false;
        commentToUpdateId = default;
    }

    private async Task UpdateCommentAsync() {
        isMakingUpdateRequest = true;
        try {
            var result = await CommentsService.UpdateCommentAsync(commentToUpdateId, updatedComment!);
            if (result.IsSuccess) {
                // update the comment on the client
                var commentToUpdate = comments!.FirstOrDefault(c => c.CommentId == commentToUpdateId);
                wantsToUpdateComment = false;
                isMakingUpdateRequest = false;
                if (commentToUpdate is not null) {
                    commentToUpdate.Content = updatedComment!.Content;
                    commentToUpdateId = default;
                }
            }
        }
        catch (OperationFailureException ex) {
            errorMessage = ex.Message;
        }
    }
}