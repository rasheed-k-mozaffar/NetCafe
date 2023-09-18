namespace NetCafe.Shared.ApiResponses;

public class ApiErrorResponse
{
    public string? Message { get; set; }
    public List<string>? Errors { get; set; }
}
