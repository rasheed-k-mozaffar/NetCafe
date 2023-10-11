using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using NetCafe.Shared.Dtos;

namespace NetCafe.Shared.Dtos;

public class PostCreateDto
{
    [Required(ErrorMessage = "The post's title is required")]
    [MaxLength(150, ErrorMessage = "The title should not be greater than 150 characters")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "The post's content is required")]
    [MaxLength(1_500_000, ErrorMessage = "The content of the post should not exceed 1,500,000 characters")]
    public string? Content { get; set; }
    public string? CoverImageUrl { get; set; }
    public Guid? SeriesId { get; set; }
    [MaxLength(3, ErrorMessage = "You can't select more than 3 tags for each post")]
    public Guid[]? TagIds { get; set; }
}
