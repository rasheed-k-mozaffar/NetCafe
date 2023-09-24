using System.ComponentModel.DataAnnotations.Schema;

namespace NetCafe.Server.Models;

public class Series
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public virtual ICollection<Post>? Posts { get; set; } = new List<Post>();
}
