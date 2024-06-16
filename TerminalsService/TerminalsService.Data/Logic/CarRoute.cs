using System.Text.Json;
using System.Text.Json.Serialization;
using TerminalsService.Data.Database;

namespace TerminalsService.Data.Logic;

public sealed class CarRoute
{
    public CarRoute()
    {
    }

    public CarRoute(CarRouteModel model)
    {
        Car = new Car() { Id = model.CarId };
        Date = model.Date;
        Route = JsonSerializer.Deserialize<List<Coordinates>>(model.RouteJson);
    }
    
    [JsonPropertyName("car")]
    public Car Car { get; set; }
    
    [JsonPropertyName("date")]
    public DateOnly Date { get; set; }
    
    [JsonPropertyName("route")]
    public List<Coordinates> Route { get; set; }
}