using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TerminalsService.Front;
using TerminalsService.Front.Helpers;
using TerminalsService.Front.Service;

namespace TerminalsService.Front
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            if (builder.HostEnvironment.IsDevelopment())
            {
                builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7226") });
            }
            else
            {
                builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://188.72.109.166:5001") });
            }
            

            builder.Services.AddScoped<IMapService, MapService>();
            builder.Services.AddScoped<IStyleHelper, StyleHelper>();

            await builder.Build().RunAsync();
        }
    }
}