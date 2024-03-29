﻿using System.Collections.Generic;
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

        public ClaimsTransformationContext(IClaimFactory claimFactory, IEnumerable<Claim> input, ClaimsTransformationFlags flags = ClaimsTransformationFlags.None) : this()
        {
            this.ClaimFactory = claimFactory;
            this.Store[ClaimStore.Initial] = input.ToList();
            this.Flags = flags;
        }

        public IDictionary<ClaimStore, IList<Claim>> Store { get; private set; }

        public IClaimFactory ClaimFactory { get; private set; }

        public ClaimsTransformationFlags Flags { get; private set; }

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
