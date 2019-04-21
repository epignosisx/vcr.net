using System.Collections.Generic;
using System.Net.Http;

namespace Vcr
{
    /// <summary>
    /// Finds the recorded http interaction that will be used for the given request.
    /// </summary>
    public interface IRequestMatcher
    {
        /// <summary>
        /// Finds match for the given request from the list of HttpInteraction or returns null.
        /// </summary>
        /// <param name="httpInteractions">Recorded HTTP interactions</param>
        /// <param name="request">Current request</param>
        /// <returns>Match or null</returns>
        HttpInteraction FindMatch(HttpInteraction[] httpInteractions, HttpRequest request);
    }
}
