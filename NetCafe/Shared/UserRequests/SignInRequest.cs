using System.ComponentModel.DataAnnotations;

namespace NetCafe.Shared.UserRequests;

public class SignInRequest
{
    [Required(ErrorMessage = "Your email is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Your password is required")]
    [MinLength(6, ErrorMessage = "The password must be at least 6 characters long")]
    [MaxLength(25, ErrorMessage = "The password should not be greater than 50 characters")]
    public string? Password { get; set; }
}
