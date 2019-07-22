using ClaimsTransformation.Engine;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ClaimsTransformation.Tests
{
    public class Utility
    {
        const string ISSUER = "ClaimsTransformation.Tests";

        public Utility()
        {
            ClaimsTransformationSettings.Defaults[ClaimProperty.ISSUER] = ISSUER;
            ClaimsTransformationSettings.Defaults[ClaimProperty.ORIGINAL_ISSUER] = ISSUER;
        }

        public bool HasIssuedClaim(IEnumerable<Claim> claims, string type, string value)
        {
            return this.GetIssuedClaims(claims, type, value).Any();
        }

        public IEnumerable<Claim> GetIssuedClaims(IEnumerable<Claim> claims, string type, string value)
        {
            return claims.Where(claim =>
            {
                return
                    string.Compare(claim.Issuer, ISSUER, true) == 0 &&
                    string.Compare(claim.OriginalIssuer, ISSUER, true) == 0 &&
                    string.Compare(claim.Type, type, true) == 0 &&
                    string.Compare(claim.Value, value, true) == 0;
            });
        }
    }
}
