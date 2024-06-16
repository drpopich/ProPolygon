using System.Text.Json.Serialization;
using TerminalsService.Data.Database;

namespace TerminalsService.Data.Logic;

public sealed class Terminal
{
    public Terminal()
    {
    }

    public Terminal(TerminalModel model)
    {
        Id = model.Id;
        Lon = model.Lon;
        Lat = model.Lat;
    }
    
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("lon")] 
    public double Lon { get; set; }

    [JsonPropertyName("lat")]
    public double Lat { get; set; }
}