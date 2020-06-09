using System;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorQuiz.FrontEnd.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Microsoft.JSInterop;

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
            builder.Services.AddSingleton<AppState>();
            builder.Services.AddSingleton<ExamService>();
            builder.Services.AddLocalization();


            var host = builder.Build();

            ClientAppSettings appSettings =  host.Services.GetService(typeof(ClientAppSettings)) as ClientAppSettings;
            await appSettings.LoadAsync();


            var jsInterop = host.Services.GetRequiredService<IJSRuntime>();
            var result = await jsInterop.InvokeAsync<string>("blazorCulture.get");
            if (result != null)
            {
                var culture = new CultureInfo(result);
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;
            }

            await host.RunAsync();
        }
    }
}
