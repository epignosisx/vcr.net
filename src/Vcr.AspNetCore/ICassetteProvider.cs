using Microsoft.AspNetCore.Http;

namespace Vcr.AspNetCore
{
    public interface ICassetteProvider
    {
        string GetCassette(HttpContext context);
    }
}
