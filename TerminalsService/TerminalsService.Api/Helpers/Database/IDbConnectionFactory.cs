using Npgsql;

namespace TerminalsService.Api.Helpers.Database;

public interface IDbConnectionFactory
{
     Task<NpgsqlConnection> CreateDbConnectionAsync();
}