namespace Vcr.AspNetCore
{
    public class VcrOptions
    {
        public string CookieName { get; set; } = "VcrState";
        public string QueryName { get; set; } = "VcrState";
        public string HeaderName { get; set; } = "VcrState";
        public IRequestMatcher RequestMatcher { get; set; } = new DefaultRequestMatcher();
    }
}
