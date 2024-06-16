using Microsoft.JSInterop;
using TerminalsService.Data.Logic;
using TerminalsService.Front.Helpers;

namespace TerminalsService.Front.Service;

public sealed class MapService : IMapService
{
    private readonly IJSRuntime _js;

    public MapService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task InitTerminalsAsync(List<Terminal> terminals)
    {
        var data = terminals.Select((terminal) => new
        {
            type = "Feature",
            properties = new
            {
                icon = "terminal",
                id = terminal.Id,
            },
            geometry = new
            {
                type = "Point",
                coordinates = new double[]{terminal.Lon, terminal.Lat}
            }
        });

        var source = new
        {
            type = "geojson",
            cluster = true,
            clusterMaxZoom = 11,
            clusterRadius = 40,
            data = new
            {
                type = "FeatureCollection",
                features = data
            }
        };

        await _js.InvokeVoidAsync("addTerminals", source);
    }

    public async Task UploadIconsAsync()
    {
        await _js.InvokeVoidAsync("uploadIcons");
    }

    public async Task AddInfoInTerminalsAsync(List<Terminal> terminals, List<TerminalMoney> terminalsMoney, DateOnly dateTrip)
    {
        var data = terminals.Select((terminal) => new
        {
            type = "Feature",
            properties = new
            {
                icon = "terminal",
                id = terminal.Id,
                money = terminalsMoney.SingleOrDefault(m => m.Terminal.Id == terminal.Id)?.Money,
                date = dateTrip.ToString()
            },
            geometry = new
            {
                type = "Point",
                coordinates = new double[]{terminal.Lon, terminal.Lat}
            }
        });

        var source = new
        {
            type = "geojson",
            cluster = true,
            clusterMaxZoom = 11,
            clusterRadius = 40,
            data = new
            {
                type = "FeatureCollection",
                features = data
            }
        };

        await _js.InvokeVoidAsync("addTerminalsInfo", source);
    }

    public async Task AddRoutes(List<CarRoute> routesList)
    {
        // Добавляем маршрут машины как отдельный слой
        foreach (var route in routesList)
        {
            Console.WriteLine(route.Car.Id);
            var data = new
            {
                type = "Feature",
                properties = new
                {
                    color = ProjectConstants.ColorDictionary[route.Car.Id]
                },
                geometry = new
                {
                    type = "LineString",
                    coordinates = route.Route.Select(x => new double[]{x.Lon, x.Lat})
                }
            };
            
            var source = new
            {
                type = "geojson",
                data = new
                {
                    type = "FeatureCollection",
                    features = new [] {data}
                }
            };

            await _js.InvokeVoidAsync("addRoute", source, route.Car.Id, ProjectConstants.ColorDictionary[route.Car.Id]);
        }
    }

    public async Task RemoveOldRoutes(List<CarRoute> routesList)
    {
        foreach (var route in routesList)
        {
            await _js.InvokeVoidAsync("removeOldRoute", route.Car.Id);
        }
    }

    public async Task RemoveOldTerminalMoney()
    {
        await _js.InvokeVoidAsync("removeOldTerminalMoney");
    }

    public async Task RemoveInitTerminal()
    {
        await _js.InvokeVoidAsync("removeInitTerminal");
    }

    public async Task NotVisibleTerminal()
    {
        await _js.InvokeVoidAsync("noneVisibleTerminal");
    }

    public async Task VisibleTerminal()
    {
        await _js.InvokeVoidAsync("visibleTerminal");
    }

    public async Task TestRoutesAsync(List<TestRoutes> routesList)
    {
        var data = routesList.Select((route) => new
        {
            type = "Feature",
            geometry = new
            {
                type = "LineString",
                coordinates = route.CoordinatesList.Select(x => new double[]{x.Lon, x.Lat})
            }
        });
        
        var source = new
        {
            type = "geojson",
            data = new
            {
                type = "FeatureCollection",
                features = data
            }
        };
        
        await _js.InvokeVoidAsync("addTestRoutes", source);
    }
}