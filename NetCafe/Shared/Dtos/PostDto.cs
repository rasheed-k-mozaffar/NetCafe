namespace NetCafe.Shared.Dtos;

public class PostDto
{
    public Guid PostId { get; set; }
    public Guid? SeriesId { get; set; }
    public SeriesDto? Series { get; set; }
    public byte[]? CoverImage { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTime PublishedOn { get; set; }
    public int Views { get; set; }
    public int Likes { get; set; }
    public ICollection<CommentDto>? Comments { get; set; }
    public ICollection<TagSummaryDto>? Tags { get; set; }
}
