namespace Vcr.AspNetCore
{
    public class CassetteState
    {
        private static readonly char[] s_delimiter = new char[] { '|' };

        public bool IsRecording { get; set; }
        public string CassetteName { get; set; }

        public static bool TryParse(string value, out CassetteState state)
        {
            state = null;
            if (string.IsNullOrEmpty(value))
                return false;

            var parts = value.Split(s_delimiter);
            
            state = new CassetteState {
                CassetteName = parts[0],
                IsRecording = parts.Length == 2 && string.Equals(parts[1], "recording", System.StringComparison.Ordinal)
            };
            return true;
        }

        public string Serialize()
        {
            return string.Concat(CassetteName, "|", IsRecording ? "recording" : "");
        }
    }
}
