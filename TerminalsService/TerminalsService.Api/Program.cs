
using TerminalsService.Api.Helpers.Database;

namespace TerminalsService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            if (app.Environment.IsDevelopment())
            {
                app.Run();
            }
            else
            {
                app.Run("http://0.0.0.0:5001");
            }
        }
    }
}