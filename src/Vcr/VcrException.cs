using System;
using System.Runtime.Serialization;

namespace Vcr
{
    [Serializable]
    public class VcrException : Exception
    {
        public VcrException()
        {
        }

        public VcrException(string message) : base(message)
        {
        }

        public VcrException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected VcrException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
