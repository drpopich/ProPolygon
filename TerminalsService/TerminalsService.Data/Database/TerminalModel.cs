namespace TerminalsService.Data.Database;

public sealed class TerminalModel
{
    public int Id { get; set; }
    
    public double Lon { get; set; }
    
    public double Lat { get; set; }
}