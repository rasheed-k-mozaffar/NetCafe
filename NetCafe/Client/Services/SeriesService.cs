
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore.Storage;
using Remotion.Linq.Clauses.ResultOperators;

namespace NetCafe.Client.Services;

public class SeriesService : ISeriesService
{
    private readonly HttpClient httpClient;

    public SeriesService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<ApiResponse> CreateSeriesAsync(SeriesCreateDto series)
    {
        var response = await httpClient.PostAsJsonAsync("/api/series", series);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new DataInsertionFailedException(message: error!.Message!);
        }

        var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
        return result!;
    }

    public async Task<ApiResponse> DeleteSeriesAsync(Guid seriesId)
    {
        var response = await httpClient.DeleteAsync($"/api/series/{seriesId}");

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new DeletionFailedException(message: error!.Message!);
        }

        var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
        return result!;
    }

    public async Task<ApiResponse<IEnumerable<SeriesSummaryDto>>> GetAllSeriesAsync()
    {
        var response = await httpClient.GetAsync("/api/series");

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new DataRetrievalFailedException(message: error!.Message!);
        }

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<SeriesSummaryDto>>>();
        return result!;
    }

    public async Task<ApiResponse<SeriesDto>> GetSeriesByIdAsync(Guid seriesId)
    {
        var response = await httpClient.GetAsync($"/api/series/{seriesId}");

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new DataRetrievalFailedException(message: error!.Message!);
        }

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<SeriesDto>>();
        return result!;
    }

    public async Task<ApiResponse> UpdateSeriesAsync(Guid seriesId, SeriesUpdateDto series)
    {
        var response = await httpClient.PutAsJsonAsync($"/api/series/{seriesId}", series);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            throw new OperationFailureException(message: error!.Message!);
        }

        var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
        return result!;
    }
}
