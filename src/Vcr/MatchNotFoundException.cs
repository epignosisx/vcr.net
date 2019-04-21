namespace Vcr
{
    public class MatchNotFoundException : VcrException
    {
        public HttpRequest UnmatchedHttpRequest { get; }

        public MatchNotFoundException(HttpRequest unmatchedHttpRequest) 
            : base($"Match no found for {unmatchedHttpRequest.Method} {unmatchedHttpRequest.Uri}")
        {
            UnmatchedHttpRequest = unmatchedHttpRequest;
        }
    }
}
