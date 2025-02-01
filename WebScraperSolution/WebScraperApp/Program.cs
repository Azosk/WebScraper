using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using WebScraperLib;

class Program
{
    static async Task Main()
    {
        var services = new ServiceCollection();
        services.AddHttpClient<IWebScraper, WebScraper>();
        var serviceProvider = services.BuildServiceProvider();
        var scraper = serviceProvider.GetService<IWebScraper>();

        if (scraper != null)
        {

            string url = "https://en.wikipedia.org/wiki/Main_Page";
            string xpath = "//div//li//a";
            // string xpath = "//div[@id='mp-itn']//li//a";


            try
            {
                var results = await scraper.ScrapeAsync(url, xpath);

                Console.WriteLine("Scraped Results:");
                if (results.Count == 0)
                {
                    Console.WriteLine("No results found. Check the XPath or website structure.");
                }
                else
                {
                    foreach (var result in results)
                    {
                        Console.WriteLine($"- {result.Text} ({result.Link})");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Failed to resolve scraper instance.");
        }
    }
}
