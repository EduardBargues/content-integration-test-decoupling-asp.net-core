
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

using Moq;

using TestImplementationDecoupling.ExternalCache;
using TestImplementationDecoupling.WebApi;

namespace TestImplementationDecoupling.Tests
{
    public class CustomWebFactory : WebApplicationFactory<Startup>
    {
        public Mock<IExternalCache> ExternalCacheMock { get; set; } = new Mock<IExternalCache>();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services => services.AddSingleton(ExternalCacheMock.Object));
        }
    }
}