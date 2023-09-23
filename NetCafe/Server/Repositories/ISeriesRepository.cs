namespace NetCafe.Server.Repositories;

public interface ISeriesRepository
{
    Task<ICollection<Series>> GetSeriesAsync();
    Task<Series> GetSeriesByIdAsync(Guid seriesId);
    Task<bool> CreateSeriesAsync(Series series);
    Task<bool> DeleteSeriesAsync(Guid seriesId);
    Task<bool> UpdateSeriesAsync();
}
