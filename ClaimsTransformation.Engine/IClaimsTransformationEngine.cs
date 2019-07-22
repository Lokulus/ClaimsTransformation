using System.Collections.Generic;
using System.Security.Claims;

namespace ClaimsTransformation.Engine
{
    public interface IClaimsTransformationEngine
    {
        IEnumerable<Claim> Transform(string rule, IEnumerable<Claim> claims);

        IEnumerable<Claim> Transform(IEnumerable<string> rules, IEnumerable<Claim> claims);
    }
}
