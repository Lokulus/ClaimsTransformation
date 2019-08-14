using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ClaimsTransformation.Engine
{
    public class ClaimsTransformationContext : IClaimsTransformationContext
    {
        private ClaimsTransformationContext()
        {
            this.Store = new Dictionary<ClaimStore, IList<Claim>>();
        }

        public ClaimsTransformationContext(IEnumerable<Claim> input) : this()
        {
            this.Store[ClaimStore.Initial] = input.ToList();
        }

        public IDictionary<ClaimStore, IList<Claim>> Store { get; private set; }

        public IEnumerable<Claim> Input
        {
            get
            {
                return this.Store.Values
                    .SelectMany(claims => claims)
                    .ToArray();
            }
        }

        public IEnumerable<Claim> Output
        {
            get
            {
                return this.Get(ClaimStore.Permanent);
            }
        }

        public IEnumerable<Claim> Get(ClaimStore store)
        {
            var claims = default(IList<Claim>);
            if (!this.Store.TryGetValue(store, out claims))
            {
                return Enumerable.Empty<Claim>();
            }
            return claims;
        }

        public void Add(ClaimStore store, IEnumerable<Claim> claims)
        {
            this.Store[store] = this.Get(store)
                .Concat(claims)
                .ToArray();
        }
    }
}
