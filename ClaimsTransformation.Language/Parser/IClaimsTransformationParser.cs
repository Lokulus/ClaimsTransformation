using ClaimsTransformation.Language.DOM;

namespace ClaimsTransformation.Language.Parser
{
    public interface IClaimsTransformationParser
    {
        RuleExpression Parse(string rule);
    }
}
