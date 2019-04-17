using System.Collections.Generic;
using System.Net.Http;

namespace Vcr
{
    public class DefaultRequestMatcher : IRequestMatcher
    {
        public HttpInteraction FindMatch(IReadOnlyList<HttpInteraction> httpInteractions, HttpRequestMessage request)
        {
            HttpInteraction bestMatch = null;
            foreach(var httpInteraction in httpInteractions)
            {
                if (httpInteraction.Request.RequestUri == request.RequestUri &&
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
