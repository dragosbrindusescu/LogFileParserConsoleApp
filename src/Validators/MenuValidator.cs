namespace LogFileParser;

public class MenuValidator
{
    public static void CheckIfOptionIsAllowed(int[] allowedOptions, int input)
    {
        if (!allowedOptions.Contains(input))
        {
            throw new FormattedError("This option is not valid!");
        }
    }
}
