using LogFileParser;

try 
{
    int userOption;

    MainMenu.DisplayTitle();
    MainMenu.DisplayDescription();

    do {
        MainMenu.DisplayMainMenu();

        string input = Console.ReadLine() ?? "";
        InputValidator.CanBeInteger(input);

        userOption = int.Parse(input);
        MenuValidator.CheckIfOptionIsAllowed(MainMenu.AllowedOptions, userOption);
    } while (userOption != 9);
} 
catch(FormattedError error) 
{
    error.DisplayErrorMessage();
} 
catch(Exception exception)
{
    FormattedError.DisplayCompleteError(exception);
}