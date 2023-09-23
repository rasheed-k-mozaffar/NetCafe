namespace NetCafe.Server.Models;

public class Series
{
    public Guid Id { get; set; }
    public Guid? ImageId { get; set; }
    public virtual Image? CoverImage { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public virtual ICollection<Post>? Posts { get; set; } = new List<Post>();
}
