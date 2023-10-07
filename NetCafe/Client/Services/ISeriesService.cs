namespace NetCafe.Client.Services;

public interface ISeriesService
{
    Task<ApiResponse<IEnumerable<SeriesSummaryDto>>> GetAllSeriesAsync();
    Task<ApiResponse<SeriesDto>> GetSeriesByIdAsync(Guid seriesId);
    Task<ApiResponse> CreateSeriesAsync(SeriesCreateDto series);
    Task<ApiResponse> DeleteSeriesAsync(Guid seriesId);
    Task<ApiResponse> UpdateSeriesAsync(Guid seriesId, SeriesUpdateDto series);
}
