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

    public static void DirectoryExists(string directoryPath)
    {
        if(!Directory.Exists(directoryPath))
        {
            throw new FormattedError("No directory found for this path");
        }
    }
}
