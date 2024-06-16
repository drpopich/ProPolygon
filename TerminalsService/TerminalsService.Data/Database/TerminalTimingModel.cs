namespace TerminalsService.Data.Database;

public sealed class TerminalTimingModel
{
    public int TerminalIdStart { get; set; }
    
    public int TerminalIdEnd { get; set; }
    
    public TimeSpan Time { get; set; }
}