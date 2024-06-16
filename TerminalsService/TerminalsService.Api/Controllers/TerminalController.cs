using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using TerminalsService.Api.Helpers.Database;
using TerminalsService.Data.Database;
using TerminalsService.Data.Logic;

namespace TerminalsService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TerminalController : ControllerBase
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public TerminalController(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        [HttpGet]
        [Route("GetAllTerminals")]
        public async Task<List<Terminal>> GetAllTerminals()
        {
            string commandText = $@"SELECT * FROM terminals";

            var result = new List<Terminal>();
            
            await using var connection = await _connectionFactory.CreateDbConnectionAsync();
            await using var command = new NpgsqlCommand(commandText, connection);
            await using var reader = await command.ExecuteReaderAsync();
            
            var idOrdinal = reader.GetOrdinal("terminal_id");
            var lonOrdinal = reader.GetOrdinal("lon");
            var latOrdinal = reader.GetOrdinal("lat");

            
            while (await reader.ReadAsync())
            {
                var terminalBd = new TerminalModel()
                {
                    Id = reader.GetInt32(idOrdinal),
                    Lon = reader.GetDouble(lonOrdinal),
                    Lat = reader.GetDouble(latOrdinal)
                };
                
                result.Add(new Terminal(terminalBd));
            }

            return result;
        }
        
        [HttpGet]
        [Route("GetTerminalMoneyForDay/{date}")]
        public async Task<List<TerminalMoney>> GetTerminalMoneyForDay(string date)
        {
            string commandText = $@"select * from terminals_money where date = '{date}'";

            var result = new List<TerminalMoney>();
            
            await using var connection = await _connectionFactory.CreateDbConnectionAsync();
            await using var command = new NpgsqlCommand(commandText, connection);
            await using var reader = await command.ExecuteReaderAsync();

            if (!reader.HasRows)
            {
                return null;
            }
            
            var idOrdinal = reader.GetOrdinal("terminal_id");
            var dateOrdinal = reader.GetOrdinal("date");
            var moneyOrdinal = reader.GetOrdinal("money");

            while (await reader.ReadAsync())
            {
                var terminalMoneyBd = new TerminalMoneyModel()
                {
                    TerminalId = reader.GetInt32(idOrdinal),
                    Date = DateOnly.FromDateTime(reader.GetDateTime(dateOrdinal)),
                    Money = reader.GetDecimal(moneyOrdinal)
                };
                    
                result.Add(new TerminalMoney(terminalMoneyBd));
            }

            return result;
        }
        
        [HttpGet]
        [Route("GetCarsRouteForDay/{date}")]
        public async Task<List<CarRoute>> GetCarsRouteForDay(string date)
        {
            string commandText = $@"select * from cars_route where date = '{date}'";

            var result = new List<CarRoute>();
            
            await using var connection = await _connectionFactory.CreateDbConnectionAsync();
            await using var command = new NpgsqlCommand(commandText, connection);
            await using var reader = await command.ExecuteReaderAsync();
            
            if (!reader.HasRows)
            {
                return null;
            }
            
            var idOrdinal = reader.GetOrdinal("car_id");
            var dateOrdinal = reader.GetOrdinal("date");
            var routeOrdinal = reader.GetOrdinal("route");
            
            while (await reader.ReadAsync())
            {
                var terminalMoneyBd = new CarRouteModel()
                {
                    CarId = reader.GetInt32(idOrdinal),
                    Date = DateOnly.FromDateTime(reader.GetDateTime(dateOrdinal)),
                    RouteJson = reader.GetString(routeOrdinal)
                };
                    
                result.Add(new CarRoute(terminalMoneyBd));
            }

            return result;
        }
        
        [HttpGet]
        [Route("GetDemoRoutes/{date}")]
        public async Task<List<TestRoutes>> GetDemoRoutes(string date)
        {
            var fileStrings = await System.IO.File.ReadAllLinesAsync(@"C:\Users\ilyap\Desktop\test-routes.csv");
            fileStrings = fileStrings.Skip(1).ToArray();
            
            var result = new List<TestRoutes>();
            
            foreach (var routesString in fileStrings)
            {
                var route = routesString.Split(';');
                
                result.Add(new TestRoutes()
                {
                    CarId = int.Parse(route[0]),
                    CoordinatesList = JsonSerializer.Deserialize<List<Coordinates>>(route[1])
                });
            }

            return result;
        }
        
        [HttpGet]
        [Route("GetDemoTerminalMoney/{date}")]
        public async Task<List<TerminalMoney>> GetDemoTerminalMoney(string date)
        {
            // string commandText = $@"SELECT * FROM terminals_money WHERE date = {date}";
            var result = new List<TerminalMoney>();
            
            var fileStrings = await System.IO.File.ReadAllLinesAsync(@"C:\Users\ilyap\Desktop\test-money.csv");
            fileStrings = fileStrings.Skip(1).ToArray();
            
            foreach (var routesString in fileStrings)
            {
                var route = routesString.Split(';');
                
                result.Add(new TerminalMoney()
                {
                    Terminal = new Terminal(){ Id = int.Parse(route[0])},
                    Date = DateOnly.FromDateTime(DateTime.Parse(route[1])),
                    Money = Decimal.Parse(route[2])
                });
            }

            return result;
        }
    }
}
