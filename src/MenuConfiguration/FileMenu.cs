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

    public static void DisplayData(string fileName, FileData previousData, FileData currentData)
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
                    previousData.GetParsingDurationAsString()
                    );
        table.AddRow("New Values", 
                    currentData.LinesProcessed, 
                    currentData.ErrorsFound, 
                    currentData.GetParsingDurationAsString()
                    );
        table.AddRow("Updated values",
                    previousData.LinesProcessed + currentData.LinesProcessed,
                    previousData.ErrorsFound + currentData.ErrorsFound,
                    previousData.AddParsingDuration(currentData.ParsingDuration).GetParsingDurationAsString()
                    );
        table.Write();
        Console.WriteLine();
    }
}
