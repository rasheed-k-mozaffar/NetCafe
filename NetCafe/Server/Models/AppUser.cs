namespace NetCafe.Server.Models;

public class AppUser : IdentityUser
{
    public string? FullName { get; set; }
    public byte[]? ProfilePicture { get; set; }
    public virtual ICollection<Comment>? Comments { get; set; } = new List<Comment>();
}
