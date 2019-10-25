using System;
using System.Runtime.Serialization;

namespace ClaimsTransformation
{
    [Serializable]
    public class ClaimsTransformationException : Exception
    {
        public ClaimsTransformationException()
        {
        }

        public ClaimsTransformationException(string message) : base(message)
        {
        }

        public ClaimsTransformationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ClaimsTransformationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
