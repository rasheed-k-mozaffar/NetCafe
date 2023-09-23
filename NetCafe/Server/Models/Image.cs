namespace NetCafe.Server;

public class Image
{
    public Guid Id { get; set; }
    public Guid? PostId { get; set; }
    public Guid? SeriesId { get; set; }
    public string? FileName { get; set; }
    public string? URL { get; set; }
}
