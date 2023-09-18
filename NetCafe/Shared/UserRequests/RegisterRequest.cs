using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NetCafe.Shared.UserRequests;

public class RegisterRequest
{
    [Required(ErrorMessage = "Your name is required")]
    public string? FullName { get; set; }

    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [RegularExpression(@"^[^\s@]+@[^\s@]+\.[^\s@]+$")]
    public string? Email { get; set; }

    public byte[]? ProfilePicture { get; set; }

    [Required(ErrorMessage = "Your password is required")]
    [MinLength(6, ErrorMessage = "The password must be at least 6 characters long")]
    [MaxLength(25, ErrorMessage = "The password must not exceed 25 characters")]
    public string? Password { get; set; }

    public bool SubscribeToNewsletter { get; set; }
}
