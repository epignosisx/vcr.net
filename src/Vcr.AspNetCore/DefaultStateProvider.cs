using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Vcr.AspNetCore
{
    internal class DefaultStateProvider : IStateProvider
    {
        private readonly IOptions<VcrOptions> _options;

        public DefaultStateProvider(IOptions<VcrOptions> options)
        {
            _options = options ?? throw new System.ArgumentNullException(nameof(options));
        }

        public void Clear(HttpContext context)
        {
            context.Response.Cookies.Delete(_options.Value.CookieName, CreateCookieOptions(context));
        }

        public CassetteState Get(HttpContext context)
        {
            CassetteState state = null;
            if (CassetteState.TryParse(context.Request.Query[_options.Value.QueryName], out state))
                return state;

            if (CassetteState.TryParse(context.Request.Headers[_options.Value.HeaderName], out state))
                return state;

            if (CassetteState.TryParse(context.Request.Cookies[_options.Value.CookieName], out state))
                return state;

            return null;
        }

        public void Set(HttpContext context, CassetteState state)
        {
            context.Response.Cookies.Append(_options.Value.CookieName, state.Serialize(), CreateCookieOptions(context));
        }

        private static CookieOptions CreateCookieOptions(HttpContext context)
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Path = "/",
                Secure = context.Request.IsHttps
            };
        }
    }
}
