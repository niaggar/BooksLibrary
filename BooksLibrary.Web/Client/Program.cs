using BooksLibrary.Web;
using BooksLibrary.Web.Contracts;
using BooksLibrary.Web.Services;
using BooksLibrary.Web.Utils;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BooksLibrary.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            ConfigureServices(builder.Services);

            await builder.Build().RunAsync();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            var apiUrl = "https://localhost:7215/api/";

            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiUrl) });
            services.AddScoped<ILoginContract, LoginService>();
            services.AddScoped<IBookContract, BookService>();

            services.AddSingleton<WebAlertsUtil>();
            services.AddSingleton<TokenUtil>();
        }
    }
}