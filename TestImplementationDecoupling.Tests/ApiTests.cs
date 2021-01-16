
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

using Moq;

using TestImplementationDecoupling.ExternalCache;
using TestImplementationDecoupling.WebApi;

using Xunit;

namespace TestImplementationDecoupling.Tests
{
    public class ApiTests : IClassFixture<CustomWebFactory>
    {
        private readonly CustomWebFactory _webFactory;
        private readonly HttpClient _httpClient;

        public ApiTests(CustomWebFactory fixture)
        {
            _webFactory = fixture;
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
        public async Task CountCharacters_OkScenarios_NoCache(string word, int expectedNumberOfCharacters)
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
            _webFactory.ExternalCacheMock.Verify(m => m.ContainsKey(word), Times.Once);
            _webFactory.ExternalCacheMock.Verify(m => m.Add(word, expectedNumberOfCharacters), Times.Once);
            _webFactory.ExternalCacheMock.Verify(m => m.GetValue(word), Times.Never);
        }

        [Theory]
        [InlineData("word", 4)]
        [InlineData("countMyLetters", 14)]
        public async Task CountCharacters_OkScenarios_Cache(string word, int expectedNumberOfCharacters)
        {
            // ARRANGE
            HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            string url = $"/{word}";
            string expectedResponse = $"The word '{word}' contains {expectedNumberOfCharacters} characters.";
            _webFactory.ExternalCacheMock
                .Setup(m => m.ContainsKey(word))
                .Returns(true);
            _webFactory.ExternalCacheMock
                .Setup(m => m.GetValue(word))
                .Returns(expectedNumberOfCharacters);

            // ACT
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            // ASSERT
            Assert.Equal(expectedStatusCode, response.StatusCode);
            string responseContent = await response.Content.ReadAsStringAsync();
            Assert.Equal(expectedResponse, responseContent);
            _webFactory.ExternalCacheMock.Verify(m => m.ContainsKey(word), Times.Once);
            _webFactory.ExternalCacheMock.Verify(m => m.Add(word, expectedNumberOfCharacters), Times.Never);
            _webFactory.ExternalCacheMock.Verify(m => m.GetValue(word), Times.Once);
            _webFactory.ExternalCacheMock.Reset();
        }
    }
}
