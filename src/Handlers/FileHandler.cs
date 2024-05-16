using System.Diagnostics;
using System.Text;

namespace LogFileParser;

public class FileHandler
{
    public static string StateDirectory(string filePath)
    {
        return $"{Directory.GetParent(Directory.GetCurrentDirectory())}/logs{Path.GetDirectoryName(filePath) ?? ""}";
    }

    public static string StateFilePath(string filePath)
    {
        return $"{StateDirectory(filePath)}/{GenerateFileName(filePath)}";
    }

    public static void InitiateStateFileData(string filePath)
    {
        WritingSummaryData(filePath, 0, 0, 0);
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

    private static void WritingSummaryData(string filePath, int processedLines, int errorsFound, int parsingDuration)
    {
        using (StreamWriter sw = new StreamWriter(filePath))
        {
            sw.WriteLine($"Lines processed: {processedLines}");
            sw.WriteLine($"Errors found: {errorsFound}");
            sw.WriteLine($"Parsing file duration (ms): {parsingDuration}");
        }
    }

    public static void CancelParsingFileIfIsTriggered(StreamReader reader, Stopwatch stopWatch, StringBuilder stateContent, StateData previousData, StateData newData, IEnumerable<string> stateFileLines, string statePath, string inputFilePath)
    {
        Console.CancelKeyPress += (sender, e) =>
        {
            Console.WriteLine("Ther process was interrupt...");

            stopWatch.Stop();

            newData.AddParsingDuration((int)stopWatch.Elapsed.TotalMilliseconds);

            Console.WriteLine("Writing log files...");
            StateDataHandler.EditSummaryData(ref stateContent, newData, stateFileLines);
            using (StreamWriter writer = new StreamWriter(statePath))
            {
                writer.WriteLine(stateContent);
            }
            //aici trebuie sa scriem fisierul 
            FileMenu.DisplayData(Path.GetFileName(inputFilePath), previousData, newData);

            // Close the file if needed
            if (reader != null)
            {
                reader.Close();
            }
            // Stop the application
            Environment.Exit(0);
        };
    }

    public static string InitiateStateFile(string inputFilePath)
    {
        string stateDirectory = StateDirectory(inputFilePath);
        string statePath = StateFilePath(inputFilePath);

        //checking if directory and file exists
        if(!Directory.Exists(stateDirectory))
        {
            Directory.CreateDirectory(stateDirectory);
        }

        if(!File.Exists(statePath))
        {
            InitiateStateFileData(statePath);
        }

        return statePath;
    }

    public static void WritingStateFile(string statePath, StateData previousData, StateData newData, Stopwatch stopWatch, StringBuilder stateContent, IEnumerable<string> stateFileLines)
    {
        if(StateDataHandler.IsDifferentState(previousData, newData))
        {
            newData.AddParsingDuration((int)stopWatch.Elapsed.TotalMilliseconds);
            Console.WriteLine("Writing log files...");
            StateDataHandler.EditSummaryData(ref stateContent, newData, stateFileLines);
            using (StreamWriter writer = new StreamWriter(statePath))
            {
                writer.WriteLine(stateContent);
            }
        }
    }
}
