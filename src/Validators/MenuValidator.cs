namespace LogFileParser;

public class MenuValidator
{
    public static void CheckIfOptionIsAllowed(Dictionary<string, int> allowedOptions, int input)
    {
        if (!allowedOptions.ContainsValue(input))
        {
            throw new FormattedError("This option is not valid!");
        }
    }
}
