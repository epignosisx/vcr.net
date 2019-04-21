using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SampleWebApp
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

            //add handlers used by the application (if any)
            handlers.Add(new AppSpecificHandler());

            //finally let VCR.net hook into the HTTP pipeline
            handlers.AddRange(additionalHandlers);

            //build finaly HttpClient
            return HttpClientFactory.Create(httpClientHandler, handlers.ToArray());
        }
    }


    /// <summary>
    /// Sample delegating handler that the app uses.
    /// ** This is not needed for VCR.net to work **
    /// </summary>
    public class AppSpecificHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Debug.WriteLine("HTTP call: {0} {1}", request.Method, request.RequestUri.ToString());
            return base.SendAsync(request, cancellationToken);
        }
    }
}
