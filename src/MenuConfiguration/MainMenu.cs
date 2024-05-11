namespace LogFileParser;

public class MainMenu
{
    public static readonly int[] AllowedOptions = [1, 2, 9];

    public static void DisplayTitle()
    {
        Console.WriteLine("");
        Console.WriteLine("*******************");
        Console.WriteLine("* Log File Parser *");
        Console.WriteLine("*******************");
        Console.WriteLine("");
    }

    public static void DisplayDescription()
    {
        Console.WriteLine("This Console Application helps you to parse log files and identify errors");
    }

    public static void DisplayMainMenu()
    {
        Console.WriteLine("");
        Console.WriteLine("Please select one choice from the list:");
        Console.WriteLine("---------------------------------------");
        Console.WriteLine("1: Parse a specific file by it path");
        Console.WriteLine("2: Parse an entire directory by it path");
        Console.WriteLine("");
        Console.WriteLine("9: Exit");
        Console.WriteLine("");
        Console.Write("Please enter a key: ");
    }
}
