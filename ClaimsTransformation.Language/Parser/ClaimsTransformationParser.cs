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
                this.OnParseError(reader);
            }
            reader.Align();
            if (!reader.EOF)
            {
                this.OnParseError(reader);
            }
            try
            {
                return ExpressionFactory.Rule(result.ForChannel(TokenChannel.Normal));
            }
            catch (ExpressionFactoryException e)
            {
                throw new ClaimsTransformationException(e.Message, e);
            }
        }

        protected virtual void OnParseError(StringReader reader)
        {
            throw new ClaimsTransformationException(
                string.Format(
                    "Could not parse expression: Position: {0}",
                    reader.Max
                )
            );
        }
    }
}
