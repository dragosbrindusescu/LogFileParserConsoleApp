using System.Diagnostics;
using System.Text;
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

        if (userOption == MainMenu.AllowedOptions[MainMenu.ParseFile])
        {
            FileMenu.DisplayMenu();
  
            //reading input file path
            string inputFilePath = @$"{Console.ReadLine() ?? ""}"; 
            PathValidator.FileExists(inputFilePath);

            //identify if file is log
            FileTypeValidator.CheckFileType(inputFilePath);

            //get log file path
            string statePath = FileHandler.InitiateStateFile(inputFilePath);
            
            //get last summary data about parsing errors
            IEnumerable<string> stateFileLines = File.ReadLines(statePath).Take(3);
            StateData previousData = StateDataHandler.GetPreviousData(stateFileLines, statePath);

            //get content of the state file
            StringBuilder stateContent = StateDataHandler.GetContent(statePath);

            //prepare to parse the input file for new data
            StateData newData = new StateData(previousData);

            //starting the stopwatch
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();            
            using (StreamReader reader = new StreamReader(inputFilePath))
            {  
                FileHandler.CancelParsingFileIfIsTriggered(
                    reader, 
                    stopWatch, 
                    stateContent, 
                    previousData, 
                    newData, 
                    stateFileLines, 
                    statePath, 
                    inputFilePath
                );
                
                //parse the file to the line where stopped before
                StateDataHandler.ResumeReader(reader, previousData);

                StateDataHandler.ParsingData(ref newData, reader, stateContent);
            }
            //stopping the stopwatch and added the time
            stopWatch.Stop();

            FileHandler.WritingStateFile(
                statePath, 
                previousData, 
                newData, 
                stopWatch, 
                stateContent, 
                stateFileLines
            );

            FileMenu.DisplayData(Path.GetFileName(inputFilePath), previousData, newData);
        }

        if(userOption == MainMenu.AllowedOptions[MainMenu.ParseDirectory])
        {
            DirectoryMenu.DisplayMenu();

            //reading input file path
            string inputDirectoryPath = @$"{Console.ReadLine() ?? ""}"; 
            PathValidator.DirectoryExists(inputDirectoryPath);
        }
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