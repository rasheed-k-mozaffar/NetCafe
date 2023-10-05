
using System.Net.Http.Json;
using Microsoft.CodeAnalysis.CSharp;

namespace NetCafe.Client.Services;

public class TagsService : ITagsService
{
    private readonly HttpClient httpClient;
    public TagsService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<ApiResponse> CreateTagAsync(TagCreateDto tag)
    {
        var response = await httpClient.PostAsJsonAsync("/api/tags", tag);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new DataInsertionFailedException(message: error!.Message!);
        }

        var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
        return result!;
    }

    public async Task<ApiResponse> DeleteTagAsync(Guid tagId)
    {
        var response = await httpClient.DeleteAsync($"/api/tags/{tagId}");

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new DeletionFailedException(message: error!.Message!);
        }

        var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
        return result!;
    }

    public async Task<ApiResponse<IEnumerable<TagSummaryDto>>> GetAllTagsAsync()
    {
        var response = await httpClient.GetAsync("/api/tags");

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new DataRetrievalFailedException(message: error!.Message!);
        }

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<TagSummaryDto>>>();
        return result!;
    }

    public async Task<ApiResponse<TagDto>> GetTagByIdAsync(Guid tagId)
    {
        var response = await httpClient.GetAsync($"/api/tags/{tagId}");

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new DataRetrievalFailedException(message: error!.Message!);
        }

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<TagDto>>();
        return result!;
    }

    public async Task<ApiResponse<TagDataDto>> GetTagDataByIdAsync(Guid tagId)
    {
        var response = await httpClient.GetAsync($"/api/tags/data/{tagId}");

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new DataRetrievalFailedException(message: error!.Message!);
        }

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<TagDataDto>>();
        return result!;
    }

    public async Task<ApiResponse> UpdateTagAsync(Guid tagId, TagUpdateDto tag)
    {
        var response = await httpClient.PutAsJsonAsync($"/api/tags/{tagId}", tag);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new OperationFailureException(message: error!.Message!);
        }

        var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
        return result!;
    }
}

