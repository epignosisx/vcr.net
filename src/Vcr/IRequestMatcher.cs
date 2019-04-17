using System.Collections.Generic;
using System.Net.Http;

namespace Vcr
{
    public interface IRequestMatcher
    {
        HttpInteraction FindMatch(IReadOnlyList<HttpInteraction> httpInteractions, HttpRequest request);
    }
}
