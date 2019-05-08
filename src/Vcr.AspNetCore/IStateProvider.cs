using Microsoft.AspNetCore.Http;

namespace Vcr.AspNetCore
{
    public interface IStateProvider
    {
        CassetteState Get(HttpContext context);
        void Set(HttpContext context, CassetteState state);
        void Clear(HttpContext context);
    }
}
