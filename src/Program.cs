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

            //get log file path
            string logFileDirectory = FileHandler.GetLogFileDirectory(inputFilePath);
            string logFilePath = FileHandler.GenerateLogFilePath(inputFilePath);

            //checking if directory and file exists
            if(!Directory.Exists(logFileDirectory))
            {
                Directory.CreateDirectory(logFileDirectory);
            }

            if(!File.Exists(logFilePath))
            {
                FileHandler.InitiateLogFileData(logFilePath);
            }

            //get last data about parsing errors
            IEnumerable<string> logFileLines = File.ReadLines(logFilePath);
            int lastLineProcessed = FileHandler.GetLastLineProcessed(logFileLines);
            int lastErrorsFound = FileHandler.CountErrorsFound(logFileLines);
            int lastParsingFileDuration = FileHandler.GetParsingDuration(logFileLines);
            
            StringBuilder errors = new StringBuilder();
            errors.AppendLine(FileHandler.GetErrors(logFileLines));

            //starting the stopwatch
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //resuming parsing the log files (it works even is the first time when parsing the file)
            IEnumerable<string> inputFileLines = File.ReadLines(inputFilePath);
            for(int i = lastLineProcessed; i < inputFileLines.Count(); i++)
            {
                lastLineProcessed += 1; 
                if(inputFileLines.ElementAt(i).Contains("error", StringComparison.CurrentCultureIgnoreCase))
                {
                    lastErrorsFound += 1;
                    errors.AppendLine(inputFileLines.ElementAt(i));
                }
            }

            //stopping the stopwatch
            stopWatch.Stop();

            int newParsingFileDuration = (int) stopWatch.Elapsed.TotalMilliseconds;
            int totalParsingFileDuration = lastParsingFileDuration + newParsingFileDuration;

            //writing the output log files with states about the parsing and display summary
            FileHandler.WriteLogFile(logFilePath, lastLineProcessed, lastErrorsFound, totalParsingFileDuration, errors.ToString());
            FileMenu.DisplayData(Path.GetFileName(inputFilePath), lastLineProcessed, lastErrorsFound, totalParsingFileDuration);
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