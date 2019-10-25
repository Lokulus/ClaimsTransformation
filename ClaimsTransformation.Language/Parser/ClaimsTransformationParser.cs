using ClaimsTransformation.Language.DOM;
using System;

namespace ClaimsTransformation.Language.Parser
{
    public class ClaimsTransformationParser : ParserBase, IClaimsTransformationParser
    {
        public ClaimsTransformationParser()
        {

        }

        public RuleExpression Parse(string rule)
        {
            var reader = new StringReader(rule);
            return this.Parse(reader);
        }

        protected virtual RuleExpression Parse(StringReader reader)
        {
            var result = default(TokenValue);
            if (!this.TryParse(reader, ClaimsTransformationSyntax.Rule, out result))
            {
                throw new NotImplementedException();
            }
            reader.Align();
            if (!reader.EOF)
            {
                throw new NotImplementedException();
            }
            return ExpressionFactory.Rule(result.ForChannel(TokenChannel.Normal));
        }
    }
}
