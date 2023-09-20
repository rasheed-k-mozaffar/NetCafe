namespace NetCafe.Server;

public static class Helpers
{
    public static async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file)
    {
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
