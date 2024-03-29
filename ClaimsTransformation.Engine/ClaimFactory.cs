﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ClaimsTransformation.Engine
{
    public class ClaimFactory : IClaimFactory
    {
        public virtual Claim Create(IEnumerable<ClaimProperty> properties)
        {
            var type = ClaimsTransformationSettings.GetDefault(ClaimProperty.TYPE);
            var value = ClaimsTransformationSettings.GetDefault(ClaimProperty.VALUE);
            var valueType = ClaimsTransformationSettings.GetDefault(ClaimProperty.VALUE_TYPE);
            var issuer = ClaimsTransformationSettings.GetDefault(ClaimProperty.ISSUER);
            var originalIssuer = ClaimsTransformationSettings.GetDefault(ClaimProperty.ORIGINAL_ISSUER);
            foreach (var property in properties)
            {
                if (property.Value == null)
                {
                    continue;
                }
                if (string.Equals(property.Name, ClaimProperty.CLAIM, StringComparison.OrdinalIgnoreCase))
                {
                    var claim = property.Value as Claim;
                    if (claim == null)
                    {
                        continue;
                    }
                    type = claim.Type;
                    value = claim.Value;
                    valueType = claim.ValueType;
                    issuer = claim.Issuer;
                    originalIssuer = claim.OriginalIssuer;
                }
                else if (string.Equals(property.Name, ClaimProperty.TYPE, StringComparison.OrdinalIgnoreCase))
                {
                    type = Convert.ToString(property.Value);
                }
                else if (string.Equals(property.Name, ClaimProperty.VALUE, StringComparison.OrdinalIgnoreCase))
                {
                    value = Convert.ToString(property.Value);
                }
                else if (string.Equals(property.Name, ClaimProperty.VALUE_TYPE, StringComparison.OrdinalIgnoreCase))
                {
                    valueType = Convert.ToString(property.Value);
                }
                else if (string.Equals(property.Name, ClaimProperty.ISSUER, StringComparison.OrdinalIgnoreCase))
                {
                    issuer = Convert.ToString(property.Value);
                }
                else if (string.Equals(property.Name, ClaimProperty.ORIGINAL_ISSUER, StringComparison.OrdinalIgnoreCase))
                {
                    originalIssuer = Convert.ToString(property.Value);
                }
            }
            if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(value))
            {
                return null;
            }
            return new Claim(type, value, valueType, issuer, originalIssuer);
        }

        public virtual IEnumerable<Claim> Create(IEnumerable<Claim> claims)
        {
            return claims.Select(claim => this.Create(claim)).ToArray();
        }

        public virtual Claim Create(Claim claim)
        {
            var properties = new[]
            {
                new ClaimProperty(ClaimProperty.TYPE, claim.Type),
                new ClaimProperty(ClaimProperty.VALUE, claim.Value),
                new ClaimProperty(ClaimProperty.VALUE_TYPE,claim.ValueType)
            };
            return this.Create(properties);
        }
    }
}
