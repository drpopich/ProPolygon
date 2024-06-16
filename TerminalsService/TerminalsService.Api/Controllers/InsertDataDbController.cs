using System.Globalization;
using System.Text.Json;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using TerminalsService.Api.Helpers.Database;
using TerminalsService.Data.Database;
using TerminalsService.Data.Logic;

namespace TerminalsService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsertDataDbController : ControllerBase
    {
        private readonly IDbConnectionFactory _connectionFactory;
        
        public InsertDataDbController(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        [HttpPost]
        [Route("InsertTerminals")]
        public async Task InsertTerminals()
        {
            var fileStrings = await System.IO.File.ReadAllLinesAsync(@"C:\Users\ilyap\Downloads\t_coords.csv");

            var terminals = fileStrings
                .Select(f => f.Split(';'))
                .Select(t => new TerminalModel()
                {
                    Id = int.Parse(t[0]),
                    Lon = double.Parse(t[1]),
                    Lat = double.Parse(t[2])
                });
            
            await using var connection = await _connectionFactory.CreateDbConnectionAsync();

            foreach (var terminal in terminals)
            {
                string commandText = $"INSERT INTO terminals (terminal_id, lon, lat) VALUES (@terminal_id, @lon, @lat)";

                await using var command = new NpgsqlCommand(commandText, connection);

                command.Parameters.AddWithValue("terminal_id", terminal.Id);
                command.Parameters.AddWithValue("lon", terminal.Lon);
                command.Parameters.AddWithValue("lat", terminal.Lat);
                
                await command.ExecuteNonQueryAsync();
            }
        }
        
        [HttpPost]
        [Route("InsertCars")]
        public async Task InsertCars()
        {
            var countCars = 50;
            
            await using var connection = await _connectionFactory.CreateDbConnectionAsync();

            for (int i = 1; i <= countCars; i++)
            {
                string commandText = $"INSERT INTO cars (car_id) VALUES (@car_id)";
                
                await using var command = new NpgsqlCommand(commandText, connection);
                command.Parameters.AddWithValue("car_id", i);
                
                await command.ExecuteNonQueryAsync();
            }
        }
        
        [HttpPost]
        [Route("InsertCarsRoute")]
        public async Task InsertCarsRoute()
        {
            var date = DateOnly.Parse("2023-12-31");
            var fileStrings = await System.IO.File.ReadAllLinesAsync(@"C:\Users\ilyap\Desktop\test-routes.csv");
            fileStrings = fileStrings.Skip(1).ToArray();
            
            var carsRouteModel = new List<CarRouteModel>();
            
            foreach (var routesString in fileStrings)
            {
                var route = routesString.Split(';');
                
                carsRouteModel.Add(new CarRouteModel()
                {
                    CarId = int.Parse(route[0]),
                    Date = date,
                    RouteJson = route[1]
                });
            }
            
            await using var connection = await _connectionFactory.CreateDbConnectionAsync();

            foreach (var carRouteModel in carsRouteModel)
            {
                string commandText = $"INSERT INTO cars_route (car_id, date, route) VALUES (@car_id, @date, @route)";

                await using var command = new NpgsqlCommand(commandText, connection);

                command.Parameters.AddWithValue("car_id", carRouteModel.CarId);
                command.Parameters.AddWithValue("date", carRouteModel.Date);
                command.Parameters.AddWithValue("route", carRouteModel.RouteJson);
                
                await command.ExecuteNonQueryAsync();
            }
        }

        [HttpPost]
        [Route("InserTerminalMoney")]
        public async Task InserTerminalMoney()
        {
            var date = DateOnly.Parse("2023-12-31");
            var terminalsMoneyModel = new List<TerminalMoneyModel>();
            
            var fileStrings = await System.IO.File.ReadAllLinesAsync(@"C:\Users\ilyap\Desktop\test-money.csv");
            fileStrings = fileStrings.Skip(1).ToArray();
            
            foreach (var routesString in fileStrings)
            {
                var route = routesString.Split(';');
                
                terminalsMoneyModel.Add(new TerminalMoneyModel()
                {
                    TerminalId =  int.Parse(route[0]),
                    Date = date,
                    Money = Decimal.Parse(route[2])
                });
            }
            
            await using var connection = await _connectionFactory.CreateDbConnectionAsync();

            foreach (var terminalMoneyModel in terminalsMoneyModel)
            {
                string commandText = $"INSERT INTO terminals_money (terminal_id, date, money) VALUES (@terminal_id, @date, @money)";

                await using var command = new NpgsqlCommand(commandText, connection);

                command.Parameters.AddWithValue("terminal_id", terminalMoneyModel.TerminalId);
                command.Parameters.AddWithValue("date", terminalMoneyModel.Date);
                command.Parameters.AddWithValue("money", terminalMoneyModel.Money);
                
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
