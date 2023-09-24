namespace NetCafe.Shared.Dtos;

public class SeriesSummaryDto
{
    public Guid SeriesId { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? PostsCount { get; set; }
}
