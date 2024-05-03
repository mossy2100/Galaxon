namespace Galaxon.Astronomy.SpaceCalendars.com.Services;

public class BufferedFileUploadService
{
    public static async Task UploadFile(IFormFile file, string dir, string newFileName)
    {
        if (file.Length == 0)
        {
            throw new IOException("File length is zero.");
        }

        try
        {
            string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, dir));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            await using FileStream fileStream =
                new(Path.Combine(path, newFileName), FileMode.Create);
            await file.CopyToAsync(fileStream);
        }
        catch (Exception ex)
        {
            throw new IOException("File copy failed.", ex);
        }
    }
}
