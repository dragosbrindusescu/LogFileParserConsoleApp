namespace LogFileParser;

public class FileTypeValidator
{
    public static readonly string[] allowedFileTypes = [".log", ".txt"];
    public static void CheckFileType(string inputFilePath)
    {
        string fileType = Path.GetExtension(inputFilePath);
        
        if(!allowedFileTypes.Contains(fileType))
        {
            throw new FormattedError("The file has not supported type for logs");
        }
    }
}
