using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsTransformation.Language.Parser
{
    [Serializable]
    public class ClaimsTransformationParserException : ClaimsTransformationException
    {
        public ClaimsTransformationParserException(string rule, int position) : base(GetMessage(rule, position))
        {
            this.Rule = rule;
            this.Position = position;
        }

        public string Rule { get; private set; }

        public int Position { get; private set; }

        protected ClaimsTransformationParserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        private static string GetMessage(string rule, int position)
        {
            return string.Format("Could not parse rule at position {0}.", position);
        }
    }
}
