using System;

namespace ClaimsTransformation.Language.DOM
{
    public class LiteralExpression : Expression
    {
        public LiteralExpression(string value)
        {
            this.Value = value;
        }

        public string Value { get; private set; }

        public override int GetHashCode()
        {
            var hashCode = 0;
            unchecked
            {
                if (!string.IsNullOrEmpty(this.Value))
                {
                    hashCode += this.Value.GetHashCode();
                }
            }
            return hashCode;
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(this.Value))
            {
                return this.Value;
            }
            else
            {
                return "{EMPTY}";
            }
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as LiteralExpression);
        }

        public virtual bool Equals(LiteralExpression other)
        {
            if (other == null)
            {
                return false;
            }
            if (object.ReferenceEquals(this, other))
            {
                return true;
            }
            if (!string.Equals(this.Value, other.Value, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            return true;
        }
    }
}
