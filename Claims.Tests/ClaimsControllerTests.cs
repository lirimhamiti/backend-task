using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace Claims.Tests
{
    public class ClaimsControllerTests
    {
        [Fact]
        public async Task Get_Claims()
        {
            var cancellationToken = TestContext.Current.CancellationToken;

            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(_ =>
                {});

            var client = application.CreateClient();

            var response = await client.GetAsync("/Claims", cancellationToken);

            response.EnsureSuccessStatusCode();



            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
            var body = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken);
            Assert.Equal(JsonValueKind.Array, body.ValueKind);
        }

    }
}
