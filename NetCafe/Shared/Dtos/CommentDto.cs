namespace NetCafe.Shared.Dtos;

public class CommentDto
{
    public Guid CommentId { get; set; }
    //public Guid? ParentCommentId { get; set; }
    public Guid PostId { get; set; }
    public string? AppUserId { get; set; }
    public string? UserName { get; set; }
    public string? Content { get; set; }
    public int Likes { get; set; }
    public DateTime PostedOn { get; set; }
    //public ICollection<CommentDto>? Replies { get; set; }
}
