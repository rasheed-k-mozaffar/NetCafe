using System.Net.Http.Json;
using NetCafe.Shared;

namespace NetCafe.Client.Services;

public class PostsService : IPostsService
{
    private readonly HttpClient httpClient;

    public PostsService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<ApiResponse> CreatePostAsync(PostCreateDto post)
    {
        var response = await httpClient.PostAsJsonAsync("/api/posts", post);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new DataInsertionFailedException(message: error!.Message!);
        }
        var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
        return result!;
    }

    public async Task<ApiResponse> DeletePostAsync(Guid postId)
    {
        var response = await httpClient.DeleteAsync($"/api/posts/{postId}");

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new DeletionFailedException(message: error!.Message!);
        }
        var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
        return result!;
    }

    public async Task<ApiResponse<IEnumerable<PostSummaryDto>>> GetAllPostsAsync()
    {
        var response = await httpClient.GetAsync("/api/posts");

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new DataRetrievalFailedException(message: error!.Message!);
        }
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<PostSummaryDto>>>();
        return result!;
    }

    public async Task<ApiResponse<PostDto>> GetPostByIdAsync(Guid postId)
    {
        var response = await httpClient.GetAsync($"/api/posts/{postId}");

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new DataRetrievalFailedException(message: error!.Message!);
        }
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<PostDto>>();
        return result!;
    }

    public async Task<ApiResponse> UpdatePostAsync(Guid postId, PostUpdateDto post)
    {
        var response = await httpClient.PutAsJsonAsync($"/api/posts/{postId}", post);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new OperationFailureException(message: error!.Message!);
        }
        var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
        return result!;
    }
}
