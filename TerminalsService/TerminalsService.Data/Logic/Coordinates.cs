using System.Text.Json.Serialization;
using CsvHelper.Configuration.Attributes;

namespace TerminalsService.Data.Logic;

public sealed class Coordinates
{
    [JsonPropertyName("lon")]
    public double Lon { get; set; }

    [JsonPropertyName("lat")]
    public double Lat { get; set; }
}