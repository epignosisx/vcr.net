using System.Net.Http;

namespace Vcr
{
    public class HttpInteraction
    {
        public bool Played { get; set; }
        public HttpRequestMessage Request { get; set; }
        public HttpResponseMessage Response { get; set; }
    }
}
