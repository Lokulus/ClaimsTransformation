using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace ClaimsTransformation.Engine
{
    public interface IClaimsTransformationEngine
    {
        ClaimsTransformationFlags Flags { get; }

        IEnumerable<Claim> Transform(string rule, IEnumerable<Claim> claims);

        IEnumerable<Claim> Transform(IEnumerable<string> rules, IEnumerable<Claim> claims);
    }

    [Flags]
    public enum ClaimsTransformationFlags : byte
    {
        None = 0,
        /// <summary>
        /// When this flag is set, EXISTS([]) always evaulates to True regardless of whether any input was provided.
        /// </summary>
        UnconditionalExistsIsAlwaysTrue = 1
    }
}
