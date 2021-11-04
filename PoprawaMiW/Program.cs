using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PoprawaMiW.Helpers;
using PoprawaMiW.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PoprawaMiW
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<ILoadDataService, LoadDataService>();
            builder.Services.AddScoped<IDisplayDataService, DisplayDataService>();
            builder.Services.AddScoped<IFileDataService, FileDataService>();
            builder.Services.AddScoped<INormalizeService, NormalizeService>();
            builder.Services.AddScoped<IKnnService, KnnService>();

            await builder.Build().RunAsync();
        }
    }
}
