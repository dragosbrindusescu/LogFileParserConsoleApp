namespace LogFileParser;

public class DirectoryHandler
{
    private static readonly string[] allowedExtensions = [".txt", ".log"];
    public static IEnumerable<string> GetFiles(string directory)
    {
        return  Directory.EnumerateFiles(directory, "*.*", SearchOption.AllDirectories)
                                             .Where(file => allowedExtensions.Contains(Path.GetExtension(file), StringComparer.OrdinalIgnoreCase));
    }
}
