using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Vcr.AspNetCore
{
    public class VcrMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICassetteProvider _cassetteProvider;

        public VcrMiddleware(RequestDelegate next, ICassetteProvider cassetteProvider)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _cassetteProvider = cassetteProvider ?? throw new ArgumentNullException(nameof(cassetteProvider));
        }

        public async Task Invoke(HttpContext context)
        {
            var storage = new FileSystemCassetteStorage(new System.IO.DirectoryInfo(""));
            var vcr = new VCR(storage);
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

    public class HttpContextVcrProvider : IVcrProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextVcrProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public VCR GetVcr()
        {
            return (VCR)_httpContextAccessor.HttpContext.Items["Vcr"];
        }
    }

    public interface ICassetteProvider
    {
        string GetCassette(HttpContext context);
    }
}
