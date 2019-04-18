using YamlDotNet.Serialization;

namespace Vcr
{
    public class HttpBody
    {
        [YamlMember(Order = 1)]
        public string Encoding { get; set; }

        [YamlMember(Order = 2, ScalarStyle = YamlDotNet.Core.ScalarStyle.Literal)]
        public string String { get; set; }
    }
}
