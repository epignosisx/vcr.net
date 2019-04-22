# VCR.net
Record your .NET Core test suite's HTTP interactions and replay them during future test runs for fast, deterministic, accurate tests.

Inspired by [Ruby's VCR](https://github.com/vcr/vcr) project.

## NuGet
Install the [VCR.net](https://www.nuget.org/packages/VCR.net) package from NuGet to your integration tests project.

## How does it work?
VCR.net taps into the `HttpClient` delegating handlers pipeline to record HTTP interactions and then replay them in future tests. This has a few implications:

1- All instances of `HttpClient` need to have VCR.net's `DelegatingHandler` in order for it to record and playback HTTP traffic. Instead of sprinking VCR.net usage everywhere in your codebase, our recommendation is that you centralize the creation of `HttpClient` and make it DI friendly. More on this below.

2- While the core library does not have any ties to .NET Core, the reality is that .NET Framework uses other APIs for HTTP communication like `HttpWebRequest` and friends which are not supported.

### VCR.net integration into your existing integration tests using ASP.NET Core TestHost
To integrate into your ASP.NET Core integration tests, first ensure you have a way to tap into all HttpClient instances created by the app. Libraries consumed by the app, usually give you the option to provide the HttpClient to use.

One way of doing it is by registering the `HttpClient` in DI and let all consumers take it from DI:

In Startup.cs:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
    
    // Register HttpClient with DI to easily override it in integration tests project.
    services.AddSingleton<HttpClient>(AppHttpClientFactory.Create());
}
```

In consuming code:

```csharp
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SampleWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("/")]
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetStringAsync("https://httpbin.org/get");
            return Content(response);
        }
    }
}
```

Then from your integration tests, override the `HttpClient` in DI with a new one containing the VCR.net's delegating handler:

```csharp
public class HomeControllerTest : IClassFixture<WebApplicationFactory<SampleWebApp.Startup>>
{
    private readonly VCR _vcr;
    private readonly WebApplicationFactory<Startup> _factory;

    public HomeControllerTest(WebApplicationFactory<SampleWebApp.Startup> factory)
    {
        //The directory created should be included in source control
        //to allow future test runs to be playbacked and not recorded.
        var dirInfo = new System.IO.DirectoryInfo("../../../cassettes"); //3 levels up to get to the root of the test project
        _vcr = new VCR(new FileSystemCassetteStorage(dirInfo));

        _factory = factory.WithWebHostBuilder(c =>
        {
            c.ConfigureTestServices(services =>
            {
                //Override application's HttpClient to use VCR's delegating handler.
                var vcrHandler = _vcr.GetVcrHandler();
                services.AddSingleton<HttpClient>(AppHttpClientFactory.Create(vcrHandler));
            });
        });
    }
```

And finally wrap your test in the cassette you would like to use to record and playback HTTP interactions:

```cs
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
```

[Full sample can be found here.](https://github.com/epignosisx/vcr.net/tree/master/sample)

