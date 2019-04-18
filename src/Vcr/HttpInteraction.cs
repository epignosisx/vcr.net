using YamlDotNet.Serialization;

namespace Vcr
{
    public class HttpInteraction
    {
        [YamlIgnore]
        public bool Played { get; set; }

        [YamlMember(Order = 1)]
        public HttpRequest Request { get; set; }

        [YamlMember(Order = 2)]
        public HttpResponse Response { get; set; }
    }
}
