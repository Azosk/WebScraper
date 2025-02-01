using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using WebScraperLib;
using Xunit;

public class WebScraperTests
{
    [Fact]
    public async Task ScrapeAsync_ValidHtml_ReturnsResults()
    {
        // Arrange
        string testHtml = "<html><body><a class='titlelink' href='https://example.com'>Example Title</a></body></html>";

        // Mock HttpMessageHandler (since HttpClient calls this internally)
        var handlerMock = new Mock<HttpMessageHandler>();

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync", // Correct method to mock
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(testHtml)
            });

        var httpClient = new HttpClient(handlerMock.Object);
        var scraper = new WebScraper(httpClient);

        // Act
        var results = await scraper.ScrapeAsync("https://fakeurl.com", "//a[@class='titlelink']");

        // Assert
        Assert.NotNull(results);
        Assert.Single(results);
        Assert.Equal("Example Title", results[0].Text);
        Assert.Equal("https://example.com", results[0].Link);
    }
}
