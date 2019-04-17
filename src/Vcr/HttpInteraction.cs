using System.Net.Http;

namespace Vcr
{
    public class HttpInteraction
    {
        public bool Played { get; set; }
        public HttpRequest Request { get; set; }
    }
}
