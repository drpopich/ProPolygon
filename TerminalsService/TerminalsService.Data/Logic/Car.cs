using System.Text.Json.Serialization;
using CsvHelper.Configuration.Attributes;

namespace TerminalsService.Data.Logic;

public sealed class Car
{
    [JsonPropertyName("id")]
    [Name("car_id")]
    public int Id { get; set; }
}