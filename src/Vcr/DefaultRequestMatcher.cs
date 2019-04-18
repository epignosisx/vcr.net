using System.Collections.Generic;
using System.Linq;

namespace Vcr
{
    public class DefaultRequestMatcher : IRequestMatcher
    {
        public HashSet<string> IgnoreHeaders { get; } = new HashSet<string> { "Date" };

        public HttpInteraction FindMatch(IReadOnlyList<HttpInteraction> httpInteractions, HttpRequest request)
        {
            HttpInteraction bestMatch = null;
            foreach(var httpInteraction in httpInteractions)
            {
                if (httpInteraction.Played)
                    continue;

                if (httpInteraction.Request.Uri != request.Uri)
                    continue;

                if (httpInteraction.Request.Method != request.Method)
                    continue;

                if (!HeadersEqual(httpInteraction.Request, request))
                    continue;

                if (!BodysEqual(httpInteraction.Request, request))
                    continue;

                if (!httpInteraction.Played)
                    return httpInteraction;
            }

            return bestMatch;
        }

        protected virtual bool HeadersEqual(HttpRequest recordedRequest, HttpRequest request)
        {
            foreach (var recordedHeader in recordedRequest.Headers)
            {
                if (IgnoreHeaders.Contains(recordedHeader.Key))
                    return false;

                if (!request.Headers.TryGetValue(recordedHeader.Key, out var headerValues))
                    return false;

                if (recordedHeader.Value.Count != headerValues.Count)
                    return false;

                if (recordedHeader.Value.Except(headerValues).Any())
                    return false;
            }

            return true;
        }

        protected virtual bool BodysEqual(HttpRequest recordedRequest, HttpRequest request)
        {
            if (recordedRequest.Body == null && request.Body == null)
                return true;

            if (recordedRequest.Body == null)
                return false;

            if (request.Body == null)
                return false;

            return string.Equals(recordedRequest.Body.Encoding, request.Body.Encoding) &&
                string.Equals(recordedRequest.Body.String, request.Body.String);
        }
    }
}
