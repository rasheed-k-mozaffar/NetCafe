namespace NetCafe.Shared.Dtos;

public class PostDto
{
    public Guid PostId { get; set; }
    public Guid? SeriesId { get; set; }
    public SeriesSummaryDto? Series { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTime PublishedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public int Views { get; set; }
    public int Likes { get; set; }
    public ICollection<CommentDto>? Comments { get; set; }
    public ICollection<TagSummaryDto>? Tags { get; set; }
}
