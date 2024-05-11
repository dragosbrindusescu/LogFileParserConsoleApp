namespace LogFileParser;

public class FormattedError: Exception
{
    public FormattedError(): base() {}
    public FormattedError(string message): base(message) {}
    public FormattedError(string message, Exception inner) : base(message, inner) {}
    public static void DisplayCompleteError(Exception exception)
    {
        Console.WriteLine("");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error Message: {exception.Message}");
        Console.WriteLine($"Stack Trace: {exception.StackTrace}");
        Console.ResetColor();
    }

    public void DisplayErrorMessage()
    {
        Console.WriteLine("");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(Message);
        Console.ResetColor();
    }
}
