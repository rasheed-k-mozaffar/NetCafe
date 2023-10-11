namespace NetCafe.Shared.Dtos;

public class PostSummaryDto
{
    public Guid PostId { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? Title { get; set; }
    public int Likes { get; set; }
    public bool IsPublished { get; set; }
    public DateTime PublishedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public List<TagSummaryDto>? Tags { get; set; }
}
