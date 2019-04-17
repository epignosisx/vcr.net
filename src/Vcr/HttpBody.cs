using SharpYaml.Serialization;

namespace Vcr
{
    public class HttpBody
    {
        [YamlMember(1)]
        public string Encoding { get; set; }

        [YamlMember(2)]
        public string String { get; set; }
    }
}
