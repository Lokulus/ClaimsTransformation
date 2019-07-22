using ClaimsTransformation.Language.DOM;

namespace ClaimsTransformation.Engine
{
    public interface IClaimsTransformationCache
    {
        void Add(string rule, RuleExpression expression);

        bool TryGetValue(string rule, out RuleExpression expression);
    }
}
