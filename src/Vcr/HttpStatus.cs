using SharpYaml.Serialization;

namespace Vcr
{
    public class HttpStatus
    {
        [YamlMember(1)]
        public int Code { get; set; }
        [YamlMember(2)]
        public string Message { get; set; }
    }
}
