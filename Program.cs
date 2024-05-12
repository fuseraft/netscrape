using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Netscrape.Helpers;
using Netscrape.Services.Interfaces;
using Netscrape.Services.Implementations;

namespace Netscrape;

class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length == 0 || args.Any(arg => new string[] { "--help", "-help", "/help", "--h", "-h", "/h", "/?" }.Contains(arg.ToLower())))
        {
            PrintUsage();
            return;
        }

        using IHost host = CreateHost(args);
        await ExecuteScrapingAsync(host, args[0]);
    }

    private static IHost CreateHost(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                services.AddHttpClient();
                services.AddTransient<IHttpClientFacade, HttpClientFacade>();
                services.AddTransient<NetScrape>();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Warning);
            })
            .Build();

    private static async Task ExecuteScrapingAsync(IHost host, string query)
    {
        var netScrape = host.Services.GetRequiredService<NetScrape>();
        try
        {
            var htmlDocument = await netScrape.LoadHtmlFromUrlAsync($"https://www.google.com/search?q={query.Replace(" ", "+")}");
            netScrape.PrintAllLinks(htmlDocument);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static void PrintUsage() => Console.WriteLine("usage: netscrape \"search terms\"");
}