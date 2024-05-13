using System.Text;

namespace LogFileParser;

public class FileMenu
{
    public static void DisplayMenu()
    {
        Console.WriteLine("");
        Console.Write("Please enter the file path: ");
    }

    public static void DisplayData(string fileName, int lastLineProcessed, int lastErrorsFound, int parsingDuration)
    {
        TimeSpan timeSpan = TimeSpan.FromMilliseconds(parsingDuration);

        Console.WriteLine("");
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine(fileName);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < fileName.Length; i++)
        {
            sb.Append('-');
        }
        Console.WriteLine(sb.ToString());
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine($"Processed lines: {lastLineProcessed}");
        Console.WriteLine($"Errors found: {lastErrorsFound}");
        Console.WriteLine($"Time elapsed: {timeSpan.Minutes}m {timeSpan.Seconds}s {timeSpan.Milliseconds}ms");
        Console.ResetColor();
    }
}
