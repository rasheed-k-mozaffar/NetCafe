namespace NetCafe.Shared.Dtos;

public class PostSummaryDto
{
    public Guid PostId { get; set; }
    public byte[]? CoverImage { get; set; }
    public string? Title { get; set; }
    public int Likes { get; set; }
    public DateTime PublishedOn { get; set; }
    public List<TagDto>? Tags { get; set; }
}
