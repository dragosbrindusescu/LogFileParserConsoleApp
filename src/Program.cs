﻿using LogFileParser;

int userOption = -1;

MainMenu.DisplayTitle();
MainMenu.DisplayDescription();

do
{
    MainMenu.DisplayMainMenu();

    try
    {
        string input = Console.ReadLine() ?? "";
        InputValidator.CanBeInteger(input);

        userOption = int.Parse(input);
        MenuValidator.CheckIfOptionIsAllowed(MainMenu.AllowedOptions, userOption);

        if (userOption == MainMenu.AllowedOptions[MainMenu.ParseFile])
        {
            FileMenu.DisplayMenu();

            //reading input file path
            string inputFilePath = @$"{Console.ReadLine() ?? ""}";
            PathValidator.FileExists(inputFilePath);

            //identify if file is log
            FileTypeValidator.CheckFileType(inputFilePath);

            //parse the input file and log errors
            FileHandler.ParseFileAndLogErrors(inputFilePath);
        }

        if (userOption == MainMenu.AllowedOptions[MainMenu.ParseDirectory])
        {
            DirectoryMenu.DisplayMenu();

            //reading input file path
            string inputDirectoryPath = @$"{Console.ReadLine() ?? ""}";

            // Get all files in the directory
            IEnumerable<string> files = DirectoryHandler.GetFiles(inputDirectoryPath);

            foreach (string file in files)
            {
                FileHandler.ParseFileAndLogErrors(file);
            }
        }
    }
    catch (FormattedError error)
    {
        error.DisplayErrorMessage();
    }
    catch (Exception exception)
    {
        FormattedError.DisplayCompleteError(exception);
    }
} while (userOption != 9);