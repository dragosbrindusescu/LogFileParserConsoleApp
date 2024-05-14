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
            FileData lastFileData = FileHandler.GetPreviousData(logFileLines, logFilePath);

            //starting the stopwatch
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            FileData newFileData = new FileData(0, 0, 0);
            //resuming parsing the log files (it works even is the first time when parsing the file)
            
            using (StreamReader reader = new StreamReader(inputFilePath))
            {   
                long offset = lastFileData.LinesProcessed; // Offsetul dorit

                // Mutați cursorul la locația dorită în fișier
                reader.BaseStream.Seek(offset, SeekOrigin.Begin);

                // Ignoră datele tamponate care ar putea fi citite deja
                reader.DiscardBufferedData();

                // Deschide fișierul de tip log pentru scriere
                using (StreamWriter writer = new StreamWriter(logFilePath))
                {
                    Console.WriteLine(newFileData.LinesProcessed);
                    int lineCounter = 0;

                    Console.CancelKeyPress += (sender, e) =>
                    {
                        // În acest moment putem efectua orice operațiuni necesare de curățare
                        // înainte de încheierea aplicației.
                        Console.WriteLine("Ther process was interrupt...");

                        Console.WriteLine(newFileData.LinesProcessed);
                        Console.WriteLine(lineCounter);

                        // Închidem fișierul, dacă a fost deschis
                        if (reader != null)
                        {
                            writer.Close();
                            reader.Close();
                        }

                        // Oprim aplicația
                        Environment.Exit(0);
                    };

                    string line;                    // Parcurge fiecare linie din fișierul original
                    while ((line = reader.ReadLine()) != null)
                    {
                        newFileData.IncrementLinesProcessed();
                        lineCounter++;
                        // Verifică dacă linia conține cuvântul "error"
                        if (line.Contains("error", StringComparison.CurrentCultureIgnoreCase))
                        {
                            newFileData.IncrementErrorsFound();
                            // Scrie linia în fișierul de tip log
                            writer.WriteLine($"Line {lineCounter}: {line}");
                            // Asigură că scrierea este efectivă în fișierul de log
                            writer.Flush();
                        }
                    }
                }
            }
            // IEnumerable<string> inputFileLines = File.ReadLines(inputFilePath);
            // for(int i = lastFileData.LinesProcessed; i < inputFileLines.Count(); i++)
            // {
            //     newFileData.IncrementLinesProcessed();
            //     if(inputFileLines.ElementAt(i).Contains("error", StringComparison.CurrentCultureIgnoreCase))
            //     {
            //         newFileData.IncrementErrorsFound();
            //         newFileData.AppendError(inputFileLines.ElementAt(i));
            //     }
            // }

            // //stopping the stopwatch
            // stopWatch.Stop();

            // newFileData.ParsingDuration = (int) stopWatch.Elapsed.TotalMilliseconds;

            // FileHandler.WriteLogFile(logFilePath,
            //                         lastFileData.LinesProcessed + newFileData.LinesProcessed, 
            //                         lastFileData.ErrorsFound + newFileData.ErrorsFound, 
            //                         lastFileData.ParsingDuration + newFileData.ParsingDuration,
            //                         $"{lastFileData.Errors}\n{newFileData.Errors}"
            //                         );
            // FileMenu.DisplayData(Path.GetFileName(inputFilePath), lastFileData, newFileData);
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