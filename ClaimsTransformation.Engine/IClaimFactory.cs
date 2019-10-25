using System.Collections.Generic;
using System.Security.Claims;

namespace ClaimsTransformation.Engine
{
    public interface IClaimFactory
    {
        Claim Create(Claim claim);

        Claim Create(IEnumerable<ClaimProperty> properties);

        IEnumerable<Claim> Create(IEnumerable<Claim> claims);
    }
}
