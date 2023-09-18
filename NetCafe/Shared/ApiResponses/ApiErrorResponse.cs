namespace NetCafe.Shared;

public class ApiErrorResponse
{
    public string? Message { get; set; }
    public List<string>? Errors { get; set; }
}
