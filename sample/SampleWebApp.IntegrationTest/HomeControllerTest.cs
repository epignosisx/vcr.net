using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Vcr;
using Xunit;

namespace SampleWebApp.IntegrationTest
{
    public class HomeControllerTest : IClassFixture<WebApplicationFactory<SampleWebApp.Startup>>
    {
        private readonly VCR _vcr;
        private readonly WebApplicationFactory<Startup> _factory;

        public HomeControllerTest(WebApplicationFactory<SampleWebApp.Startup> factory)
        {
            var dirInfo = new System.IO.DirectoryInfo("../../../cassettes"); //3 levels up to get to the root of the test project
            _vcr = new VCR(new FileSystemCassetteStorage(dirInfo));

            _factory = factory.WithWebHostBuilder(c =>
            {
                c.ConfigureTestServices(services =>
                {
                    //Override application's HttpClient to use VCR's delegating handler
                    var vcrHandler = _vcr.GetVcrHandler();
                    vcrHandler.InnerHandler = new HttpClientHandler();
                    services.AddSingleton<HttpClient>(new HttpClient(vcrHandler));
                });
            });
        }

        [Fact]
        public async Task Returns200Ok()
        {
            using (_vcr.UseCassette("home_page", RecordMode.Once))
            {
                //arrange
                var client = _factory.CreateClient();

                //act
                var response = await client.GetAsync("/");

                //assert
                Assert.True(response.IsSuccessStatusCode);
            }
        }
    }
}
