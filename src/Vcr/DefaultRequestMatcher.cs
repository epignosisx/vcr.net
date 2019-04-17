using System.Collections.Generic;
using System.Net.Http;

namespace Vcr
{
    public class DefaultRequestMatcher : IRequestMatcher
    {
        public HttpInteraction FindMatch(IReadOnlyList<HttpInteraction> httpInteractions, HttpRequest request)
        {
            HttpInteraction bestMatch = null;
            foreach(var httpInteraction in httpInteractions)
            {
                if (httpInteraction.Request.Uri == request.Uri &&
                    httpInteraction.Request.Method == request.Method)
                {
                    if (!httpInteraction.Played)
                        return httpInteraction;

                    bestMatch = httpInteraction;
                }
            }

            return bestMatch;
        }
    }
}
