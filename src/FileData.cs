using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace LogFileParser;

public class FileData 
{
    private int linesProcessed;
    private int errorsFound;
    private int parsingDuration;
    private StringBuilder errors;
    private string filePath;
    public FileData(int linesProcessed, int errorsFound, int parsingDuration, string errors = "", string filePath = "")
    {
        this.linesProcessed = linesProcessed;
        this.errorsFound = errorsFound;
        this.parsingDuration = parsingDuration;
        this.errors = new StringBuilder(errors);
        this.filePath = filePath;
    }
    public int LinesProcessed
    {
        get {return linesProcessed;}   
        set {linesProcessed = value;}
    }
    public int ErrorsFound 
    {
        get {return errorsFound;}
        set {errorsFound = value;}
    }
    public int ParsingDuration
    {
        get {return parsingDuration;}
        set {parsingDuration = value;}
    }
    public string Errors
    {
        get {return errors.ToString();}    
        set {errors = new StringBuilder(value);}
    }
    public string FilePath
    {
        get {return filePath;}
        set {filePath = value;}
    } 
    public void IncrementLinesProcessed()
    {
        linesProcessed++;
    }

    public void IncrementErrorsFound()
    {
        errorsFound++;
    }

    public string GetParsingDurationAsString()
    {
        TimeSpan ts = TimeSpan.FromMilliseconds(ParsingDuration);

        return $"{ts.Minutes}m {ts.Seconds}s {ts.Milliseconds}ms";
    }

    public FileData AddParsingDuration(int miliseconds)
    {
        parsingDuration += miliseconds;
        return this;
    }
    public void AppendError(string line)
    {
        errors.AppendLine(line);
    }
}
