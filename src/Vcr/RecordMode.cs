namespace Vcr
{
    public enum RecordMode
    {
        /// <summary>
        /// Do not record any HTTP interactions; play them back.
        /// </summary>
        None,
        /// <summary>
        /// Record the HTTP interactions if the cassette has not already been recorded;
        /// otherwise, playback the HTTP interactions.
        /// </summary>
        Once,
        /// <summary>
        /// Playback previously recorded HTTP interactions and record new ones.
        /// </summary>
        NewEpisodes,
        /// <summary>
        /// Record every HTTP interactions; do not play any back.
        /// </summary>
        All
    }
}
