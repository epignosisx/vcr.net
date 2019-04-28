using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Vcr
{
    internal delegate Task<HttpResponseMessage> HttpCallAsync(HttpRequestMessage request, CancellationToken cancellationToken);

    public class VcrHandler : DelegatingHandler
    {
        private readonly IVcrProvider _vcrProvider;
        private readonly 

        public VcrHandler(IVcrProvider vcrProvider)
        {
            _vcrProvider = vcrProvider;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var vcr = _vcrProvider.GetVcr();
            return vcr.Cassette.HandleRequestAsync(base.SendAsync, request, cancellationToken);
        }
    }

    public interface IVcrProvider
    {
        VCR GetVcr();
    }
}
