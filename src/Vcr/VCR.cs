using System.Net.Http;

namespace Vcr
{
    public class VCR : IVcrProvider
    {
        public ICasseteStorage Storage { get; set; }

        public IRequestMatcher DefaultMatcher { get; set; } = new DefaultRequestMatcher();

        internal Cassette Cassette { get; private set; }

        /// <summary>
        /// Cassette Storage to use.
        /// </summary>
        /// <param name="storage">Storage implementatino to use</param>
        public VCR(ICasseteStorage storage)
        {
            Storage = storage ?? throw new System.ArgumentNullException(nameof(storage));
        }

        /// <summary>
        /// Creates or uses cassette with given name to record HTTP interactions.
        /// </summary>
        /// <param name="name">Cassette name</param>
        /// <param name="recordMode">Rercording mode</param>
        /// <param name="requestMatcher">Request matcher to use with this cassette</param>
        /// <returns></returns>
        public Cassette UseCassette(string name, RecordMode recordMode = RecordMode.Once, IRequestMatcher requestMatcher = null)
        {
            Cassette = new Cassette(Storage, name, recordMode, requestMatcher ?? DefaultMatcher);
            return Cassette;
        }

        /// <summary>
        /// HttpClient delegading handler that must be added to HttpClient middleware pipeline to record and playback HTTP interactions.
        /// </summary>
        /// <returns></returns>
        public DelegatingHandler GetVcrHandler()
        {
            return new VcrHandler(this);
        }

        public VCR GetVcr()
        {
            return this;
        }
    }
}
