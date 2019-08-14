using System.Collections.Generic;
using System.Security.Claims;

namespace ClaimsTransformation.Engine
{
    public interface IClaimsTransformationContext
    {
        IEnumerable<Claim> Input { get; }

        IEnumerable<Claim> Output { get; }

        IEnumerable<Claim> Get(ClaimStore store);

        void Add(ClaimStore store, IEnumerable<Claim> claims);

    }

    public enum ClaimStore
    {
        None,
        Initial,
        Transient,
        Permanent
    }
}
