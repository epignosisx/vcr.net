using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Vcr
{
    internal delegate Task<HttpResponseMessage> HttpCallAsync(HttpRequestMessage request, CancellationToken cancellationToken);

    public class VcrHandler : DelegatingHandler
    {
        public IVcrProvider VcrProvider { get; set; }

        public VcrHandler()
        {
        }

        public VcrHandler(IVcrProvider vcrProvider)
        {
            VcrProvider = vcrProvider;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var vcr = VcrProvider?.GetVcr();
            if (vcr == null)
                return base.SendAsync(request, cancellationToken);

            return vcr.Cassette.HandleRequestAsync(base.SendAsync, request, cancellationToken);
        }
    }
}
