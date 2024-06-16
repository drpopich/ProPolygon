using System.Text.Json.Serialization;
using TerminalsService.Data.Database;

namespace TerminalsService.Data.Logic;

public sealed class TerminalMoney
{
    public TerminalMoney()
    {
    }

    public TerminalMoney(TerminalMoneyModel model)
    {
        Terminal = new Terminal() { Id = model.TerminalId };
        Date = model.Date;
        Money = model.Money;
    }
    
    [JsonPropertyName("terminal")]
    public Terminal Terminal { get; set; }
    
    [JsonPropertyName("date")]
    public DateOnly Date { get; set; }
    
    [JsonPropertyName("money")]
    public decimal Money { get; set; }
}