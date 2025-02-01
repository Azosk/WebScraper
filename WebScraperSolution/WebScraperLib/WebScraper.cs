using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace WebScraperLib
{
    public interface IWebScraper
    {
        Task<List<ScrapedResult>> ScrapeAsync(string url, string xpath);
    }

    public class ScrapedResult
    {
        public string Text { get; set; }
        public string Link { get; set; }
    }

    public class WebScraper : IWebScraper
    {
        private readonly HttpClient _httpClient;

        public WebScraper(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<List<ScrapedResult>> ScrapeAsync(string url, string xpath)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(xpath))
                throw new ArgumentException("URL and XPath must be provided.");

            try
            {
                var response = await _httpClient.GetStringAsync(url);
                var doc = new HtmlDocument();
                doc.LoadHtml(response);

                var nodes = doc.DocumentNode.SelectNodes(xpath);
                var results = new List<ScrapedResult>();

                if (nodes != null)
                {
                    foreach (var node in nodes)
                    {
                        results.Add(new ScrapedResult
                        {
                            Text = node.InnerText.Trim(),
                            Link = node.GetAttributeValue("href", "#")
                        });
                    }
                }

                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<ScrapedResult>();
            }
        }
    }
}
