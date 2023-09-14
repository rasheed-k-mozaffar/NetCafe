namespace NetCafe.Server.Models;

public class Post
{
    public Guid Id { get; set; }
    public Guid? SeriesId { get; set; }
    public virtual Series? Series { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTime PublishedOn { get; set; }
    public int Views { get; set; }
    public int Likes { get; set; }
    public virtual ICollection<Comment>? Comments { get; set; } = new List<Comment>();
    public virtual ICollection<Tag>? Tags { get; set; } = new List<Tag>();
}
