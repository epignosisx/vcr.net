using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SampleWebAppUsingVcr
{
    public static class AppHttpClientFactory
    {
        public static HttpClient Create(params DelegatingHandler[] additionalHandlers)
        {
            //create and configure httpClientHandler as needed.
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.UseCookies = false;

            //create HttpClient delegating handler pipeline
            List<DelegatingHandler> handlers = new List<DelegatingHandler>();

            //finally let VCR.net hook into the HTTP pipeline
            handlers.AddRange(additionalHandlers);

            //build finaly HttpClient
            return HttpClientFactory.Create(httpClientHandler, handlers.ToArray());
        }
    }
}
