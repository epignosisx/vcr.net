using System.Net.Http;
using SharpYaml.Serialization;

namespace Vcr
{
    public class HttpInteraction
    {
        [YamlIgnore]
        public bool Played { get; set; }

        [YamlMember(1)]
        public HttpRequest Request { get; set; }

        [YamlMember(2)]
        public HttpResponse Response { get; set; }
    }
}
