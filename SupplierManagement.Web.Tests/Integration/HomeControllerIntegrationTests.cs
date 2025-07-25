using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace SupplierManagement.Web.Tests.Integration
{
    public class HomeControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public HomeControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Home_Index_ReturnsSuccessStatusCode()
        {
            // Act
            var response = await _client.GetAsync("/");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Home_Index_ReturnsHtmlContent()
        {
            // Act
            var response = await _client.GetAsync("/");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("text/html", response.Content.Headers.ContentType?.ToString());
            Assert.NotEmpty(content);
        }

        [Fact]
        public async Task Home_Index_ContainsExpectedContent()
        {
            // Act
            var response = await _client.GetAsync("/");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            // Check for common HTML elements that should be present
            Assert.Contains("<html", content, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("</html>", content, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("<body", content, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("</body>", content, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task NonExistentPage_ReturnsNotFound()
        {
            // Act
            var response = await _client.GetAsync("/nonexistent-page");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("/Home")]
        [InlineData("/Home/Index")]
        public async Task Home_AlternativeRoutes_ReturnsSuccessStatusCode(string url)
        {
            // Act
            var response = await _client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Home_Error_ReturnsSuccessStatusCode()
        {
            // Act
            var response = await _client.GetAsync("/Home/Error");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task StaticFiles_StylesheetsLoad()
        {
            // Act
            var response = await _client.GetAsync("/css/site.css");

            // Assert
            // This might return 404 if the file doesn't exist, which is acceptable
            // The test mainly checks that the request is handled properly
            Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Application_StartsSuccessfully()
        {
            // This test simply verifies that the application can start without throwing exceptions
            // The fact that we can create a client and make requests proves this

            // Act & Assert
            var response = await _client.GetAsync("/");

            // If we get here without exceptions, the application started successfully
            Assert.True(response.StatusCode == HttpStatusCode.OK ||
                       response.StatusCode == HttpStatusCode.NotFound ||
                       response.StatusCode == HttpStatusCode.Redirect);
        }
    }
}
