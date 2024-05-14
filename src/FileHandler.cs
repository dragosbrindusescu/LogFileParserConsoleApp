using System.Text;

namespace LogFileParser;

public class FileHandler
{
    public static readonly string[] SummaryLines = ["Lines processed: ", "Errors found: ", "Parsing file duration: "];

    public static FileData GetPreviousData(IEnumerable<string> logFileLines, string filePath = "")
    {
        int lastLinesProcessed = GetLastLineProcessed(logFileLines);
        int lastErrorsFound = CountErrorsFound(logFileLines);
        int lastParsingFileDuration = GetParsingDuration(logFileLines);
        string errors = GetErrors(logFileLines);
        return new FileData(lastLinesProcessed, lastErrorsFound, lastParsingFileDuration, errors, filePath);
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

    public static void WriteLogFile(string filePath, int processedLines, int errorsFound, int parsingDuration, string errors = "")
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

                //tried to parse line by line and insert real time data, but it doesn't worked
                //I think this happened because of StreamWriter
                // string[] lines = errors.Split(new string[] { "\n", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                // foreach(string line in lines)
                // {
                //     sw.WriteLine(line);
                //     sw.Flush();
                // }
            }
        }
    }

    private static int GetLastLineProcessed(IEnumerable<string> logFileLines)
    {
        return GetLineInformation(logFileLines, FileSummaryEnum.LinesProcessed);
    }

    private static int CountErrorsFound (IEnumerable<string> logFileLines)
    {
        return GetLineInformation(logFileLines, FileSummaryEnum.ErrorsFound);
    }

    private static int GetParsingDuration(IEnumerable<string> logFileLines)
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

    private static string GenerateFileName(string filePath)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(Path.GetFileNameWithoutExtension(filePath) ?? "");
        stringBuilder.Append($"_{Path.GetExtension(filePath).Substring(1) ?? ""}");
        stringBuilder.Append("_info.log");
        stringBuilder.Replace(" ", "_");

        return stringBuilder.ToString().ToLower();
    }
}
