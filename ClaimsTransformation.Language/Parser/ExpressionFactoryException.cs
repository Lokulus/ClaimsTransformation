using System;
using System.Runtime.Serialization;

namespace ClaimsTransformation.Language.Parser
{
    [Serializable]
    public class ExpressionFactoryException : Exception
    {
        public ExpressionFactoryException()
        {
        }

        public ExpressionFactoryException(string message) : base(message)
        {
        }

        public ExpressionFactoryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExpressionFactoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}