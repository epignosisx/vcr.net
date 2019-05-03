using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Vcr.AspNetCore
{
    public class VcrMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICassetteProvider _cassetteProvider;
        private readonly ICasseteStorage _casseteStorage;

        public VcrMiddleware(RequestDelegate next, ICassetteProvider cassetteProvider, ICasseteStorage casseteStorage)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _cassetteProvider = cassetteProvider ?? throw new ArgumentNullException(nameof(cassetteProvider));
            _casseteStorage = casseteStorage ?? throw new ArgumentNullException(nameof(casseteStorage));
        }

        public async Task Invoke(HttpContext context)
        {
            var vcr = new VCR(_casseteStorage);
            var cassette = _cassetteProvider.GetCassette(context);

            if (string.IsNullOrEmpty(cassette))
            {
                await _next(context);
                return;
            }

            using (vcr.UseCassette(cassette))
            {
                context.Items["Vcr"] = vcr;
                await _next(context);
            }
        }
    }
}
