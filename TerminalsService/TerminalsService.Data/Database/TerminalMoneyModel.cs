namespace TerminalsService.Data.Database;

public sealed class TerminalMoneyModel
{
    public int TerminalId { get; set; }
    
    public DateOnly Date { get; set; }
    
    public decimal Money { get; set; }
}