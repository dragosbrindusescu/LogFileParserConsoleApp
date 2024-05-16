using System.Text;
using ConsoleTables;

namespace LogFileParser;

public class FileMenu
{
    private static readonly string[] columns = [" ", "Lines processed", "Errors found", "Duration"];
    public static void DisplayMenu()
    {
        Console.WriteLine("");
        Console.Write("Please enter the file path: ");
    }

    public static void DisplayData(string fileName, StateData previousData, StateData currentData)
    {
        Console.WriteLine("");
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine(fileName);
        Console.ResetColor();

        ConsoleTableOptions consoleTableOptions = new ConsoleTableOptions
        {
            EnableCount = false,
            Columns = columns
        };

        var table = new ConsoleTable(consoleTableOptions);
        table.AddRow("Previous values", 
                    previousData.LinesProcessed, 
                    previousData.ErrorsFound, 
                    StateData.GetParsingDurationAsString(previousData.ParsingDuration)
                    );
        table.AddRow("New Values",
                    currentData.LinesProcessed - previousData.LinesProcessed,
                    currentData.ErrorsFound - previousData.ErrorsFound,
                    StateData.GetParsingDurationAsString(currentData.ParsingDuration - previousData.ParsingDuration)
                    );   
        table.AddRow("Total", 
                    currentData.LinesProcessed, 
                    currentData.ErrorsFound, 
                    StateData.GetParsingDurationAsString(currentData.ParsingDuration)
                    );
        table.Write();
        Console.WriteLine();
    }
}
