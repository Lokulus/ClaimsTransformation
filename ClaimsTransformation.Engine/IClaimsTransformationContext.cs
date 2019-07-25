using System.Collections.Generic;
using System.Security.Claims;

namespace ClaimsTransformation.Engine
{
    public interface IClaimsTransformationContext
    {
        IEnumerable<Claim> Input { get; set; }

        IEnumerable<Claim> Output { get; set; }
    }
}
