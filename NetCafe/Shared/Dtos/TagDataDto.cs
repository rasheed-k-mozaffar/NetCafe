using System.Data.Common;

namespace NetCafe.Shared.Dtos;

public class TagDataDto
{
    public Guid TagId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public ICollection<PostSummaryDto>? Posts { get; set; }
}
