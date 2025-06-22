namespace Jazer.Server.Extensions;

public static class FormFileExtensions
{
    public static bool IsPng(this IFormFile file)
    {
        if (!file.ContentType.Equals("image/png", StringComparison.OrdinalIgnoreCase))
            return false;

        byte[] pngSignature = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A];
    
        using var stream = file.OpenReadStream();
    
        var header = new byte[pngSignature.Length];
        stream.ReadExactly(header, 0, pngSignature.Length);
    
        stream.Position = 0;
    
        return header.SequenceEqual(pngSignature);
    }
}