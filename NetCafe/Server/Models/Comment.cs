namespace NetCafe.Server.Models;

public class Comment
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public string? Content { get; set; }
    public int Likes { get; set; }
    public DateTime PostedOn { get; set; }
    public virtual ICollection<Comment> Replies { get; set; } = new List<Comment>();
    public virtual Post? Post { get; set; }
}