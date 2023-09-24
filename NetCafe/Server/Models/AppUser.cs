namespace NetCafe.Server.Models;

public class AppUser : IdentityUser
{
    public string? FullName { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public virtual ICollection<Comment>? Comments { get; set; } = new List<Comment>();
}
