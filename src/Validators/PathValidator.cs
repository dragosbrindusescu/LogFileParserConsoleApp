namespace LogFileParser;

public class PathValidator
{
    public static void FileExists(string filePath)
    {
        if(!File.Exists(filePath))
        {
            throw new FormattedError("No file found for this path");
        }
    }
}
