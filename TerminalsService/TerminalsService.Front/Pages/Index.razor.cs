using System.Net;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using TerminalsService.Data.Logic;

namespace TerminalsService.Front.Pages;

public partial class Index
{
    private List<Terminal> _initTerminals;
    private List<CarRoute> _routes = new();
    private List<TerminalMoney> _terminalMoney = new();
    
    private bool isInitTerminal = true;

    private List<IBrowserFile> loadedFiles = new();
    
    private string activeSettingBtnTerminal = "setting-btn-active";
    private bool isActiveTerminal = true;
    
    private readonly IDictionary<string, object>? _additionalAttributes = new Dictionary<string, object>() { {"accept", ".csv"} };
    private DateOnly _dateTripReady = DateOnly.FromDateTime(DateTime.Now);
    private DateOnly _dateTripСalc= DateOnly.FromDateTime(DateTime.Now);

    protected override async void OnInitialized()
    {
        // Получение всех терминалов
        var responce = await _httpClient.GetAsync("api/Terminal/GetAllTerminals");
        _initTerminals = await responce.Content.ReadFromJsonAsync<List<Terminal>>();

        await _mapService.UploadIconsAsync();
        await _mapService.InitTerminalsAsync(_initTerminals);
    }
    
    protected override async void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            await _js.InvokeVoidAsync("initMap");
        }
    }

    private async Task ViewResultAsync()
    {
        // Удаляем прошлые маршруты
        if (_routes.Count != 0)
        {
            await _mapService.RemoveOldRoutes(routesList: _routes);
        }

        if (_terminalMoney.Count != 0)
        {
            await _mapService.RemoveOldTerminalMoney();
        }

        if (isInitTerminal)
        {
            await _mapService.RemoveInitTerminal();
            isInitTerminal = false;
        }
        
        // Запрашиваем деньги для выбранного дня
        var responceMoney = await _httpClient.GetAsync($"api/Terminal/GetTerminalMoneyForDay/{_dateTripReady.ToString("yyyy-M-d")}");
        if (responceMoney.StatusCode == HttpStatusCode.NoContent)
        {
            // todo: добовлять сообщение на экран
            return;
        }
        _terminalMoney = await responceMoney.Content.ReadFromJsonAsync<List<TerminalMoney>>();
        
        // Запрашиваем маршруты для выбранного дня
        var responceRoute = await _httpClient.GetAsync($"api/Terminal/GetCarsRouteForDay/{_dateTripReady.ToString("yyyy-M-d")}");
        if (responceRoute.StatusCode == HttpStatusCode.NoContent)
        {
            // todo: добовлять сообщение на экран
            return;
        }
        _routes = await responceRoute.Content.ReadFromJsonAsync<List<CarRoute>>();

        // Обновляем точки на карте и добавляем худ с инфо
        await _mapService.AddInfoInTerminalsAsync(
            terminals: _initTerminals, 
            terminalsMoney: _terminalMoney,
            dateTrip: _dateTripReady);
        
        // Добавляем маршруты на карту
        await _mapService.AddRoutes(routesList: _routes);
    }

    private async Task SendForCalculationsAsync()
    {
        
    }

    private async Task OnInputFileChange(InputFileChangeEventArgs e)
    {
        long maxAllowedSize = 1024 * 1024;
        foreach (var file in e.GetMultipleFiles(1))
        {
            // валидировать размер файла
            if (file.Size > maxAllowedSize)
            {
                Console.WriteLine("Файл большой");
                break;
            }
            loadedFiles.Add(file);
        }
    }

    private async Task StyleTerminal()
    {
        if (isActiveTerminal)
        {
            await _mapService.NotVisibleTerminal();
            isActiveTerminal = false;
            activeSettingBtnTerminal = "";
        }
        else
        {
            await _mapService.VisibleTerminal();
            isActiveTerminal = true;
            activeSettingBtnTerminal = "setting-btn-active";
        }
    }

    private async Task StyleCarRoute(int carId)
    {
        await _js.InvokeVoidAsync("styleCarRouteBtn", carId);
    }
}