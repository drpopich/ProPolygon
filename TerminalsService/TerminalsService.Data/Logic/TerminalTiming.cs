using System.Text.Json.Serialization;

namespace TerminalsService.Data.Logic;

public sealed class TerminalTiming
{
    [JsonPropertyName("terminal_start")]
    public Terminal TerminalStart { get; set; }
    
    [JsonPropertyName("terminal_end")]
    public Terminal TerminalEnd { get; set; }
    
    [JsonPropertyName("time")]
    public TimeSpan Time { get; set; }
}