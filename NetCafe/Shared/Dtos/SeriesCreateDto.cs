using System.ComponentModel.DataAnnotations;

namespace NetCafe.Shared.Dtos;

public class SeriesCreateDto
{
    public Guid SeriesId { get; set; }
    [Required(ErrorMessage = "The series name is required")]
    [MinLength(2, ErrorMessage = "Series' name must contain at least 2 letters")]
    [MaxLength(100, ErrorMessage = "Series' name should not be greater than 100 characters")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "The series' description is required")]
    [MinLength(5, ErrorMessage = "Series' description must contain at least 5 characters")]
    [MaxLength(500, ErrorMessage = "Series' description should not be greater than 500 characters")]
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
}
