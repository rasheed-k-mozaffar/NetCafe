
using System.Net.Http.Json;

namespace NetCafe.Client.Services;

public class CommentsService : ICommentsService
{
    private readonly HttpClient httpClient;

    public CommentsService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<ApiResponse> AddCommentAsync(Guid postId, CommentCreateDto comment)
    {
        var response = await httpClient.PostAsJsonAsync($"/api/comments/{postId}", comment);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new DataInsertionFailedException(message: error!.Message!);
        }

        var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
        return result!;
    }

    public async Task<ApiResponse> AddReplyToCommentAsync(Guid commentId, CommentCreateDto reply)
    {
        var response = await httpClient.PostAsJsonAsync($"/api/comments/reply/{commentId}", reply);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new DataInsertionFailedException(message: error!.Message!);
        }

        var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
        return result!;
    }

    public async Task<ApiResponse> DeleteCommentAsync(Guid commentId)
    {
        var response = await httpClient.DeleteAsync($"/api/comments/{commentId}");

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new DeletionFailedException(message: error!.Message!);
        }

        var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
        return result!;
    }

    public async Task<ApiResponse<CommentDto>> GetCommentByIdAsync(Guid commentId)
    {
        var response = await httpClient.GetAsync($"/api/comments/{commentId}");

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new DataRetrievalFailedException(message: error!.Message!);
        }

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<CommentDto>>();
        return result!;
    }

    public async Task<ApiResponse<IEnumerable<CommentDto>>> GetCommentsForPostAsync(Guid postId)
    {
        var response = await httpClient.GetAsync($"/api/comments/{postId}");

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new DataRetrievalFailedException(message: error!.Message!);
        }

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<CommentDto>>>();
        return result!;
    }

    public async Task<ApiResponse> UpdateCommentAsync(Guid commentId, CommentUpdateDto comment)
    {
        var response = await httpClient.PutAsJsonAsync($"/api/comments/{commentId}", comment);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new OperationFailureException(message: error!.Message!);
        }

        var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
        return result!;
    }
}

