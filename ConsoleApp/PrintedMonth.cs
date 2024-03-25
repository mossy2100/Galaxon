namespace Galaxon.ConsoleApp;

public class PrintedMonth()
{
    public List<string> Lines = new ();

    public void AddLine(string line)
    {
        Lines.Add(line);
    }

    public string GetLine(int lineNumber)
    {
        try
        {
            return Lines[lineNumber];
        }
        catch (ArgumentOutOfRangeException)
        {
            return "                            ";
        }
    }
}
