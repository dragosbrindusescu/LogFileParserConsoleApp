using System.Text;

namespace LogFileParser;

public class FileHandler
{
    public static readonly string[] SummaryLines = ["Lines processed: ", "Errors found: ", "Parsing file duration: "];
    private static string GenerateFileName(string filePath)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(Path.GetFileNameWithoutExtension(filePath) ?? "");
        stringBuilder.Append($"_{Path.GetExtension(filePath).Substring(1) ?? ""}");
        stringBuilder.Append("_info.log");
        stringBuilder.Replace(" ", "_");

        return stringBuilder.ToString().ToLower();
    }

    public static string GetLogFileDirectory(string filePath)
    {
        return $"{Directory.GetParent(Directory.GetCurrentDirectory())}/logs{Path.GetDirectoryName(filePath) ?? ""}";
    }

    public static string GenerateLogFilePath(string filePath)
    {
        return $"{GetLogFileDirectory(filePath)}/{GenerateFileName(filePath)}";
    }

    public static void InitiateLogFileData(string filePath)
    {
        WriteLogFile(filePath, 0, 0, 0);
    }

    public static void WriteLogFile(string filePath, int processedLines, int errorsFound, int parsingDuration, string? errors = "")
    {
        using (StreamWriter sw = new StreamWriter(filePath))
        {
            sw.WriteLine($"Lines processed: {processedLines}");
            sw.WriteLine($"Errors found: {errorsFound}");
            sw.WriteLine($"Parsing file duration (ms): {parsingDuration}");
            if(!string.IsNullOrEmpty(errors))
            {
                sw.WriteLine("");
                sw.WriteLine(errors);
            }
        }
    }

    public static int GetLastLineProcessed(IEnumerable<string> logFileLines)
    {
        return GetLineInformation(logFileLines, FileSummaryEnum.LinesProcessed);
    }

    public static int CountErrorsFound (IEnumerable<string> logFileLines)
    {
        return GetLineInformation(logFileLines, FileSummaryEnum.ErrorsFound);
    }

    public static int GetParsingDuration(IEnumerable<string> logFileLines)
    {
        return GetLineInformation(logFileLines, FileSummaryEnum.ParsingFileDuration);
    }

    public static string GetErrors(IEnumerable<string> logFileLines)
    {
        StringBuilder errors = new StringBuilder();
        for (int i = (int)FileSummaryEnum.Errors; i < logFileLines.Count(); i++)
        {
            errors.AppendLine(logFileLines.ElementAt(i));
        }

        return errors.ToString();
    }

    private static int GetLineInformation(IEnumerable<string> logFileLines, FileSummaryEnum lineNumber)
    {
        string line = logFileLines.ElementAt((int)lineNumber);
        int startIndex = line.IndexOf(':') + 1;

        return int.Parse(line.Substring(startIndex).Trim());
    }
}
