using System.Net.Http;

namespace Vcr
{
    public class VCR
    {
        public ICasseteStorage Storage { get; set; }

        public IRequestMatcher DefaultMatcher { get; set; } = new DefaultRequestMatcher();

        internal Cassette Cassette { get; private set; }

        public Cassette UseCassette(string name, RecordMode recordMode = RecordMode.Once, IRequestMatcher requestMatcher = null)
        {
            Cassette = new Cassette(Storage, name, recordMode, requestMatcher ?? DefaultMatcher);
            return Cassette;
        }

        public DelegatingHandler GetVcrHandler()
        {
            return new VcrHandler(this);
        }
    }
}
