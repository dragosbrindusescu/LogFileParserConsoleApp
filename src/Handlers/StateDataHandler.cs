using System.Text;

namespace LogFileParser;

public class StateDataHandler
{
    public static readonly string[] SummaryLines = ["Lines processed: ", "Errors found: ", "Parsing file duration: "];

    public static void EditSummaryData(ref StringBuilder summaryData, StateData stateData, IEnumerable<string> stateFileLines)
    {
        summaryData.Replace(stateFileLines.ElementAt((int)FileSummaryEnum.LinesProcessed), $"{SummaryLines[(int)FileSummaryEnum.LinesProcessed]}{stateData.LinesProcessed}");
        summaryData.Replace(stateFileLines.ElementAt((int)FileSummaryEnum.ErrorsFound), $"{SummaryLines[(int)FileSummaryEnum.ErrorsFound]}{stateData.ErrorsFound}");
        summaryData.Replace(stateFileLines.ElementAt((int)FileSummaryEnum.ParsingFileDuration), $"{SummaryLines[(int)FileSummaryEnum.ParsingFileDuration]}{stateData.ParsingDuration}");
    }
    
    public static StateData GetPreviousData(IEnumerable<string> logFileLines, string filePath = "")
    {
        int lastLinesProcessed = GetLastLineProcessed(logFileLines);
        int lastErrorsFound = CountErrorsFound(logFileLines);
        int lastParsingFileDuration = GetParsingDuration(logFileLines);

        return new StateData(lastLinesProcessed, lastErrorsFound, lastParsingFileDuration, filePath);
    }

    public static bool IsDifferentState(StateData previousState, StateData newState)
    {
        return newState.LinesProcessed - previousState.LinesProcessed != 0;
    }

    public static void ParsingData(ref StateData newData, StreamReader reader, StringBuilder stateContent)
    {
        string line;           
        while ((line = reader.ReadLine()) != null)
        {
            newData.IncrementLinesProcessed();
            // Verifică dacă linia conține cuvântul "error"
            if (line.Contains("error", StringComparison.CurrentCultureIgnoreCase))
            {
                newData.IncrementErrorsFound();
                stateContent.AppendLine(line);
            }
        }
    }

    public static void ResumeReader(StreamReader reader, StateData previousData)
    {
        for (long i = 0; i < previousData.LinesProcessed; i++)
        {
            reader.ReadLine();
        }
    }

    public static StringBuilder GetContent(string statePath)
    {
        StringBuilder stateContent = new StringBuilder();
        using (StreamReader reader = new StreamReader(statePath))
        {
            stateContent.Append(reader.ReadToEnd());
        }

        return stateContent;
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

    private static int GetLineInformation(IEnumerable<string> logFileLines, FileSummaryEnum lineNumber)
    {
        string line = logFileLines.ElementAt((int)lineNumber);
        int startIndex = line.IndexOf(':') + 1;

        return int.Parse(line.Substring(startIndex).Trim());
    }
}
