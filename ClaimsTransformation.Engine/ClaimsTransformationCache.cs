using ClaimsTransformation.Language.DOM;
using System;
using System.Collections.Generic;

namespace ClaimsTransformation.Engine
{
    public class ClaimsTransformationCache : CappedDictionary<string, RuleExpression>, IClaimsTransformationCache
    {
        public static readonly int CAPACITY = 1024;

        public static readonly IEqualityComparer<string> COMPARER = StringComparer.OrdinalIgnoreCase;

        public ClaimsTransformationCache() : base(CAPACITY, COMPARER)
        {

        }
    }
}
