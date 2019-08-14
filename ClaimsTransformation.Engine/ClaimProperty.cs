using System;
using System.Collections.Generic;

namespace ClaimsTransformation.Engine
{
    public class ClaimProperty
    {
        public const string CLAIM = "Claim";

        public const string ISSUER = "Issuer";

        public const string ORIGINAL_ISSUER = "OriginalIssuer";

        public const string TYPE = "Type";

        public const string VALUE = "Value";

        public const string VALUE_TYPE = "ValueType";

        public const string CONDITION = "Condition";

        public const string ISSUANCE = "Issuance";

        public ClaimProperty(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }

        public string Name { get; private set; }

        public object Value { get; set; }

        public static IEnumerable<IEnumerable<ClaimProperty>> Productize(IEnumerable<ClaimProperty> properties)
        {
            var groups = new Dictionary<string, List<ClaimProperty>>(StringComparer.OrdinalIgnoreCase);
            foreach (var property in properties)
            {
                var group = default(List<ClaimProperty>);
                if (!groups.TryGetValue(property.Name, out group))
                {
                    group = new List<ClaimProperty>();
                    groups.Add(property.Name, group);
                }
                group.Add(property);
            }
            return groups.Values.CartesianProduct();
        }
    }
}
