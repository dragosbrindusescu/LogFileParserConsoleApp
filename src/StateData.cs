using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace LogFileParser;

public class StateData 
{
    private int linesProcessed;
    private int errorsFound;
    private int parsingDuration;
    private string filePath;
    public StateData(StateData stateData)
    {
        linesProcessed = stateData.linesProcessed;
        errorsFound = stateData.errorsFound;
        parsingDuration = stateData.parsingDuration;
        filePath = stateData.filePath;
    }
    public StateData(int linesProcessed, int errorsFound, int parsingDuration, string filePath = "")
    {
        this.linesProcessed = linesProcessed;
        this.errorsFound = errorsFound;
        this.parsingDuration = parsingDuration;
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

    public static string GetParsingDurationAsString(int parsingDuration)
    {
        TimeSpan ts = TimeSpan.FromMilliseconds(parsingDuration);

        return $"{ts.Minutes}m {ts.Seconds}s {ts.Milliseconds}ms";
    }

    public StateData AddParsingDuration(int miliseconds)
    {
        parsingDuration += miliseconds;
        return this;
    }
}
