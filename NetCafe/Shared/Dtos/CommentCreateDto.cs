using System.ComponentModel.DataAnnotations;

namespace NetCafe.Shared.Dtos;

public class CommentCreateDto
{
    [Required(ErrorMessage = "You can't post an empty comment")]
    [MaxLength(1000, ErrorMessage = "The comment can't be more than 1,000 characters long")]
    public string? Content { get; set; }
}
