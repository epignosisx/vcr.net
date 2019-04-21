# VCR.net
Record your .NET Core test suite's HTTP interactions and replay them during future test runs for fast, deterministic, accurate tests.

Inspired by [Ruby's VCR](https://github.com/vcr/vcr) project.

## NuGet
Install the [VCR.net](https://www.nuget.org/packages/VCR.net/0.1.1) package from NuGet to your integration tests project.

## How does it work?
VCR.net taps into the `HttpClient` delegating handlers pipeline to record HTTP interactions and then replay them in future tests. This has a few implications:

1- All instances of `HttpClient` need to have VCR.net's `DelegatingHandler` in order for it to record and playback HTTP traffic. Instead of sprinking VCR.net usage everywhere in your codebase, our recommendation is that you centralize the creation of `HttpClient` and make it DI friendly. More on this below.

2- While the core library does not have any ties to .NET Core, the reality is that .NET Framework uses other APIs for HTTP communication like `HttpWebRequest` and friends which are not supported.

### VCR.net integration into your existing integration tests using ASP.NET Core TestHost
To integrate into your ASP.NET Core integration tests, first ensure you have a way to tap into all HttpClient instances created by the app. Libraries consumed by the app, usually give you the option to provide the HttpClient to use.

One way of doing it is by registering the `HttpClient` in DI and let all consumers take it from DI:

In Startup.cs:

https://github.com/epignosisx/vcr.net/blob/acaecd0f64601e242e9e655db35eb186d0a46ebf/sample/SampleWebApp/Startup.cs#L26-L32

In consuming code:

https://github.com/epignosisx/vcr.net/blob/acaecd0f64601e242e9e655db35eb186d0a46ebf/sample/SampleWebApp/Controllers/HomeController.cs#L16-L21

Then from your integration tests, override the `HttpClient` in DI with a new one containing the VCR.net's delegating handler:

https://github.com/epignosisx/vcr.net/blob/acaecd0f64601e242e9e655db35eb186d0a46ebf/sample/SampleWebApp.IntegrationTest/HomeControllerTest.cs#L12-L31

And finally wrap your test in the cassette you would like to use to record and playback HTTP interactions:

https://github.com/epignosisx/vcr.net/blob/acaecd0f64601e242e9e655db35eb186d0a46ebf/sample/SampleWebApp.IntegrationTest/HomeControllerTest.cs#L33-L46

[Full sample can be found here.](https://github.com/epignosisx/vcr.net/tree/master/sample)

