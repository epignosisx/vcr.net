# VCR.net
Record your test suite's HTTP interactions and replay them during future test runs for fast, deterministic, accurate tests.

Inspired by [Ruby's VCR](https://github.com/vcr/vcr) project.

VCR.net taps into the `HttpClient` delegating handlers pipeline to record HTTP interactions and then replay them in future tests.

See sample [web app](https://github.com/epignosisx/vcr.net/tree/master/sample/SampleWebApp) & [integration tests](https://github.com/epignosisx/vcr.net/tree/master/sample/SampleWebApp.IntegrationTest) project for usage.

