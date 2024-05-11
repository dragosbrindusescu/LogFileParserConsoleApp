namespace LogFileParser;

public class InputValidator
{
    public static void CanBeInteger(string value)
    {
        if (!int.TryParse(value, out int number)){
            throw new FormattedError("Input value must be an integer number");
        }
    }
}
