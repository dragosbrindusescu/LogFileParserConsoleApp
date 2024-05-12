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
  
            //citim path-ul fisierului de la tastatura
            string inputFilePath = @$"{Console.ReadLine() ?? ""}"; 
            PathValidator.FileExists(inputFilePath);

            //de indentificat daca fisierul este log. txt sau alt tip ce poate fi interpretat si citit

            //ajungem la formatul fisierului de log-uri si vedem daca exista
            string logFileDirectory = FileHandler.GetLogFileDirectory(inputFilePath);
            string logFilePath = FileHandler.GenerateLogFilePath(inputFilePath);

            // /Users/dragosbrindusescu/Desktop/error.log
            if(!Directory.Exists(logFileDirectory))
            {
                Directory.CreateDirectory(logFileDirectory);
            }

            //Daca fisierul nu exista creeam unul
            if(!File.Exists(logFilePath))
            {
                FileHandler.InitiateLogFileData(logFilePath);
            }

            //manipularea fisierelor log
            //scoaterea numarului de linii unde a ramas
            IEnumerable<string> logFileLines = File.ReadLines(logFilePath);
            int lastLineProcessed = FileHandler.GetLastLineProcessed(logFileLines);
            int lastErrorsFound = FileHandler.CountErrorsFound(logFileLines);
            int lastParsingFileDuration = FileHandler.GetParsingDuration(logFileLines);

            TimeSpan previousTs = TimeSpan.FromMilliseconds(lastParsingFileDuration);
            Console.WriteLine($"Previous processed lines: {lastLineProcessed}");
            Console.WriteLine($"Previous errors found: {lastErrorsFound}");
            Console.WriteLine($"Previous time elapsed: {previousTs.Minutes}m {previousTs.Seconds}s {previousTs.Milliseconds}ms");
            
            int newLineProcessed = 0;
            int newErrorsFound = 0;
            //parcurgerea fisierului cu loguri
            IEnumerable<string> inputFileLines = File.ReadLines(inputFilePath);

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            for(int i = lastLineProcessed; i < inputFileLines.Count(); i++)
            {
                newLineProcessed += 1; 
                if(inputFileLines.ElementAt(i).Contains("error", StringComparison.CurrentCultureIgnoreCase))
                {
                    newErrorsFound += 1;
                    Console.WriteLine(inputFileLines.ElementAt(i));
                }
            }

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            int newParsingFileDuration = (int) ts.TotalMilliseconds;

            FileHandler.WriteLogFile(logFilePath, newLineProcessed, newErrorsFound, newParsingFileDuration);

            Console.WriteLine($"Current processed lines: {newLineProcessed}");
            Console.WriteLine($"Current errors found: {newErrorsFound}");
            Console.WriteLine($"Current time elapsed: {ts.Minutes}m {ts.Seconds}s {ts.Milliseconds}ms");
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