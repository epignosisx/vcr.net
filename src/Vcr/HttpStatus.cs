using YamlDotNet.Serialization;

namespace Vcr
{
    public class HttpStatus
    {
        [YamlMember(Order = 1)]
        public int Code { get; set; }
        [YamlMember(Order = 2)]
        public string Message { get; set; }
    }
}
