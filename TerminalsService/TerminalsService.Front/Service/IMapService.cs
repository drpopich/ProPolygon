using TerminalsService.Data.Logic;

namespace TerminalsService.Front.Service;

public interface IMapService
{
    Task InitTerminalsAsync(List<Terminal> terminals);

    Task UploadIconsAsync();

    Task AddInfoInTerminalsAsync(List<Terminal> terminals, List<TerminalMoney> terminalsMoney, DateOnly dateTrip);

    Task AddRoutes(List<CarRoute> routesList);

    Task RemoveOldRoutes(List<CarRoute> routesList);

    Task RemoveOldTerminalMoney();

    Task RemoveInitTerminal();

    Task NotVisibleTerminal();

    Task VisibleTerminal();

    Task TestRoutesAsync(List<TestRoutes> routesList);
}