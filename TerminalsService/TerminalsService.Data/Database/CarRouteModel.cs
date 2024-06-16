namespace TerminalsService.Data.Database;

public sealed class CarRouteModel
{
    public int CarId { get; set; }
    
    public DateOnly Date { get; set; }
    
    public string RouteJson { get; set; }
}