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
    }
}
