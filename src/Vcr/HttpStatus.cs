using SharpYaml.Serialization;

namespace Vcr
{
    internal class HttpStatus
    {
        [YamlMember(1)]
        public int Code { get; set; }
        [YamlMember(2)]
        public string Message { get; set; }
    }
}
