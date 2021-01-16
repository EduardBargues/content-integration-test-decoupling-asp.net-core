
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

using TestImplementationDecoupling.WebApi;

using Xunit;

namespace TestImplementationDecoupling.Tests
{
    public class ApiTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;
        public ApiTests(WebApplicationFactory<Startup> fixture)
        {
            _httpClient = fixture.CreateClient();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task CountCharacters_NotFoundScenarios(string word)
        {
            // ARRANGE
            HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;
            string url = $"/{word}";

            // ACT
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            // ASSERT
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }

        [Theory]
        [InlineData("word", 4)]
        [InlineData("countMyLetters", 14)]
        public async Task CountCharacters_OkScenarios(string word, int expectedNumberOfCharacters)
        {
            // ARRANGE
            HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            string url = $"/{word}";
            string expectedResponse = $"The word '{word}' contains {expectedNumberOfCharacters} characters.";

            // ACT
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            // ASSERT
            Assert.Equal(expectedStatusCode, response.StatusCode);
            string responseContent = await response.Content.ReadAsStringAsync();
            Assert.Equal(expectedResponse, responseContent);
        }
    }
}
