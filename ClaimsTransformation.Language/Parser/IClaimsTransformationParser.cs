using ClaimsTransformation.Language.DOM;
using System.Collections.Generic;

namespace ClaimsTransformation.Language.Parser
{
    public interface IClaimsTransformationParser
    {
        IEnumerable<RuleExpression> Parse(IEnumerable<string> expressions);
    }
}
