using Vcr.AspNetCore;

namespace Microsoft.AspNetCore.Builder
{
    public static class VcrApplicationBuilderExtensions
    {
        public static void UseVcr(this IApplicationBuilder app)
        {
            app.UseMiddleware<VcrMiddleware>();
        }
    }
}
