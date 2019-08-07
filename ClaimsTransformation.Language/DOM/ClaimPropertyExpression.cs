using System;
using System.Text;

namespace ClaimsTransformation.Language.DOM
{
    public class ClaimPropertyExpression : Expression
    {
        public ClaimPropertyExpression(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }

        public override ExpressionType Type
        {
            get
            {
                return ExpressionType.ClaimPropery;
            }
        }

        public override int GetHashCode()
        {
            var hashCode = 0;
            unchecked
            {
                if (!string.IsNullOrEmpty(this.Name))
                {
                    hashCode += this.Name.GetHashCode();
                }
            }
            return hashCode;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            if (!string.IsNullOrEmpty(this.Name))
            {
                builder.Append(this.Name);
            }
            else
            {
                builder.Append("{EMPTY}");
            }
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ClaimPropertyExpression);
        }

        public virtual bool Equals(ClaimPropertyExpression other)
        {
            if (other == null)
            {
                return false;
            }
            if (object.ReferenceEquals(this, other))
            {
                return true;
            }
            if (!string.Equals(this.Name, other.Name, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            return true;
        }
    }
}
