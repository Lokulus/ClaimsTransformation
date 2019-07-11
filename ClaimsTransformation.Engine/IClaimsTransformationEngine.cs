using ClaimsTransformation.Language.DOM;
using System.Collections.Generic;
using System.Security.Claims;

namespace ClaimsTransformation.Engine
{
    public interface IClaimsTransformationEngine
    {
        IEnumerable<Claim> Transform(IEnumerable<RuleExpression> rules, IEnumerable<Claim> claims);
    }
}
