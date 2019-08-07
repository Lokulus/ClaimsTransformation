using System;
using System.Collections.Generic;

namespace ClaimsTransformation.Engine
{
    public static class ClaimsTransformationSettings
    {
        public static readonly IDictionary<string, string> Defaults = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {

        };

        internal static string GetDefault(string name)
        {
            var value = default(string);
            if (Defaults.TryGetValue(name, out value))
            {
                return value;
            }
            return default(string);
        }
    }
}
