using System;
using Microsoft.AspNetCore.Http;

namespace Vcr.AspNetCore
{
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
}
