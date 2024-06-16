using CsvHelper.Configuration.Attributes;

namespace TerminalsService.Data.Logic;

public sealed class TestRoutes
{
    public int CarId { get; set; }
    
    public List<Coordinates> CoordinatesList { get; set; }

    public string CoordinatesSting { get; set; }
}