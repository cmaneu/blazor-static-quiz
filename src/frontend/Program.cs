using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorQuiz.FrontEnd
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddSingleton<ClientAppSettings>();

            var host = builder.Build();

            ClientAppSettings appSettings =  host.Services.GetService(typeof(ClientAppSettings)) as ClientAppSettings;
            await appSettings.LoadAsync();

            await host.RunAsync();
        }
    }
}
