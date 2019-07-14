using ClaimsTransformation.Language.DOM;
using System;
using System.Collections.Generic;

namespace ClaimsTransformation.Language.Parser
{
    public class ClaimsTransformationParser : ParserBase, IClaimsTransformationParser
    {
        public ClaimsTransformationParser()
        {

        }

        public IEnumerable<RuleExpression> Parse(IEnumerable<string> expressions)
        {
            foreach (var expression in expressions)
            {
                yield return this.Parse(expression);
            }
        }

        protected virtual RuleExpression Parse(string expression)
        {
            var reader = new StringReader(expression);
            return this.Parse(reader);
        }

        protected virtual RuleExpression Parse(StringReader reader)
        {
            var result = default(TokenValue);
            if (!this.TryParse(reader, ClaimsTransformationSyntax.Rule, out result))
            {
                throw new NotImplementedException();
            }
            return ExpressionFactory.Rule(result);
        }
    }
}
