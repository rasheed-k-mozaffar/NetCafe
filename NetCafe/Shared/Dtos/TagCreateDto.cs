using System.ComponentModel.DataAnnotations;

namespace NetCafe.Shared.Dtos;

public class TagCreateDto
{
    [Required(ErrorMessage = "The tag name is required")]
    [MinLength(1, ErrorMessage = "Tag's name must contain at least one character")]
    [MaxLength(50, ErrorMessage = "Tag's name should not be greater than 50 characters")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "The tag's description is required")]
    [MinLength(10, ErrorMessage = "The description of the tag should be at least 10 characters long")]
    [MaxLength(5000, ErrorMessage = "The description of the tag should not be greater than 5,000 characters")]
    public string? Description { get; set; }
}
