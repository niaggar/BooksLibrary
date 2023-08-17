using Blazored.LocalStorage;
using BooksLibrary.Web.Contracts;
using BooksLibrary.Web.Services;
using BooksLibrary.Web.Utils;
using BooksLibrary.Web.Utils.Security;
using Microsoft.AspNetCore.Components.Authorization;
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
            services.AddScoped<IAuthContract, AuthService>();
            services.AddScoped<IBookContract, BookService>();
            services.AddScoped<AuthStateProvider>();
            services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<AuthStateProvider>());
            services.AddScoped<HttpMethodsUtil>();
            services.AddScoped<WebAlertsUtil>();
            services.AddScoped<TokenMemoryUtil>();
            services.AddBlazoredLocalStorage();
            services.AddAuthorizationCore();
        }
    }
}