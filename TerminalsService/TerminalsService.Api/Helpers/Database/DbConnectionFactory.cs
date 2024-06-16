using Npgsql;

namespace TerminalsService.Api.Helpers.Database;

public sealed class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString =
        "";
    
    public async Task<NpgsqlConnection> CreateDbConnectionAsync()
    {
        var connect = new NpgsqlConnection(_connectionString);
        await connect.OpenAsync();
        return connect;
    }
}