using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Vcr.AspNetCore
{
    public class VcrMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IStateProvider _stateProvider;
        private readonly ICasseteStorage _casseteStorage;
        private readonly IOptions<VcrOptions> _options;

        public VcrMiddleware(RequestDelegate next, IStateProvider cassetteProvider, ICasseteStorage casseteStorage, IOptions<VcrOptions> options)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _stateProvider = cassetteProvider ?? throw new ArgumentNullException(nameof(cassetteProvider));
            _casseteStorage = casseteStorage ?? throw new ArgumentNullException(nameof(casseteStorage));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public Task Invoke(HttpContext context)
        {
            if (string.Equals(context.Request.Path, "/vcr/record", StringComparison.OrdinalIgnoreCase))
            {
                return HandleRecordAsync(context);
            }

            if (string.Equals(context.Request.Path, "/vcr/stop", StringComparison.OrdinalIgnoreCase))
            {
                return HandleStopAsync(context);
            }

            var state = _stateProvider.Get(context);

            return state == null ? _next(context) : ApplyVcrAsync(context, state);
        }

        private async Task ApplyVcrAsync(HttpContext context, CassetteState state)
        {
            var vcr = new VCR(_casseteStorage);
            using (vcr.UseCassette(state.CassetteName, state.IsRecording ? RecordMode.All : RecordMode.None, _options.Value.RequestMatcher))
            {
                context.Items["Vcr"] = vcr;
                await _next(context);
            }
        }

        private Task HandleStopAsync(HttpContext context)
        {
            _stateProvider.Clear(context);
            return context.Response.WriteAsync("{\"message\":\"Stopped\"}");
        }

        private Task HandleRecordAsync(HttpContext context)
        {
            var state = _stateProvider.Get(context);
            if (state == null || string.IsNullOrEmpty(state.CassetteName))
            {
                context.Response.StatusCode = 400;
                return context.Response.WriteAsync("{\"code\":\"CassetteNameRequired\",\"message\":\"Cassette name is required\"}");
            }

            state.IsRecording = true;
            _stateProvider.Set(context, state);
            return context.Response.WriteAsync("{\"message\":\"Recording\"}");
        }
    }
}
