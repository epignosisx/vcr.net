using System.Collections.Generic;
using System.Linq;

namespace Vcr
{
    /// <summary>
    /// Default implementation of IRequestMatcher but highly configurable and overridable via inheritance. 
    /// Only matches by Uri and HTTP Method, but have options to also match on Body and Headers.
    /// </summary>
    public class DefaultRequestMatcher : IRequestMatcher
    {
        /// <summary>
        /// Whether to compare headers when finding a match or not. Default is false.
        /// </summary>
        public bool CompareHeaders { get; set; } = false;

        /// <summary>
        /// Whether to compare bodies when finding a match or not. Default is false.
        /// </summary>
        public bool CompareBody { get; set; } = false;

        /// <summary>
        /// Headers to ignore when comparing. Useful for timestamp or nonce headers.
        /// </summary>
        public HashSet<string> IgnoreHeaders { get; } = new HashSet<string>();

        public HttpInteraction FindMatch(HttpInteraction[] httpInteractions, HttpRequest request)
        {
            HttpInteraction bestMatch = null;
            foreach(var httpInteraction in httpInteractions)
            {
                if (httpInteraction.Played)
                    continue;

                if (!UriEqual(httpInteraction.Request, request))
                    continue;

                if (!MethodEqual(httpInteraction.Request, request))
                    continue;

                if (CompareHeaders && !HeadersEqual(httpInteraction.Request, request))
                    continue;

                if (CompareBody && !BodiesEqual(httpInteraction.Request, request))
                    continue;

                return httpInteraction;
            }

            return bestMatch;
        }

        protected virtual bool MethodEqual(HttpRequest recordedRequest, HttpRequest request)
        {
            return recordedRequest.Method == request.Method;
        }

        protected virtual bool UriEqual(HttpRequest recordedRequest, HttpRequest request)
        {
            return recordedRequest.Uri == request.Uri;
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

                if (!recordedHeader.Value.SequenceEqual(headerValues))
                    return false;
            }

            return true;
        }

        protected virtual bool BodiesEqual(HttpRequest recordedRequest, HttpRequest request)
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
