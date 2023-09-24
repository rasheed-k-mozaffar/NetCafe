namespace NetCafe.Shared.Dtos;

public class SeriesDto
{
    public Guid SeriesId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public List<PostSummaryDto>? Posts { get; set; }
}
