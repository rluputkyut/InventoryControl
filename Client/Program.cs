using InventoryControl.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InventoryControl.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services
               .AddScoped<IAuthenticationService, AuthenticationService>()
               .AddScoped<ILocalStorageService, LocalStorageService>();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            var host = builder.Build();

            var authenticationService = host.Services.GetRequiredService<IAuthenticationService>();
            await authenticationService.Initialize();

            await host.RunAsync();
            //await builder.Build().RunAsync();
        }
    }
}
