using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Vcr.AspNetCore
{
    public class CookieCassetteProviderOptions
    {
        public string CookieName { get; set; } = "VcrCassette";
    }

    public class CookieCassetteProvider : ICassetteProvider
    {
        private readonly IOptions<CookieCassetteProviderOptions> _options;

        public CookieCassetteProvider(IOptions<CookieCassetteProviderOptions> options)
        {
            _options = options ?? throw new System.ArgumentNullException(nameof(options));
        }

        public string GetCassette(HttpContext context)
        {
            return context.Request.Cookies[_options.Value.CookieName];
        }
    }
}
