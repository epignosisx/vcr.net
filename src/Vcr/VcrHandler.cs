using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Vcr
{
    internal delegate Task<HttpResponseMessage> HttpCallAsync(HttpRequestMessage request, CancellationToken cancellationToken);

    internal class VcrHandler : DelegatingHandler
    {
        private readonly VCR _vcr;

        public VcrHandler(VCR vcr)
        {
            _vcr = vcr;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return _vcr.Cassette.HandleRequestAsync(base.SendAsync, request, cancellationToken);
        }
    }
}
