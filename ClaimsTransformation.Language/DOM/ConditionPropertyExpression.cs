using System.Text;

namespace ClaimsTransformation.Language.DOM
{
    public class ConditionPropertyExpression : Expression
    {
        public ConditionPropertyExpression(IdentifierExpression identifier, ClaimPropertyExpression property)
        {
            this.Identifier = identifier;
            this.Property = property;
        }

        public IdentifierExpression Identifier { get; private set; }

        public ClaimPropertyExpression Property { get; private set; }

        public override ExpressionType Type
        {
            get
            {
                return ExpressionType.ConditionProperty;
            }
        }

        public override int GetHashCode()
        {
            var hashCode = 0;
            unchecked
            {
                if (this.Identifier != null)
                {
                    hashCode += this.Identifier.GetHashCode();
                }
                if (this.Property != null)
                {
                    hashCode += this.Property.GetHashCode();
                }
            }
            return hashCode;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            if (this.Identifier != null)
            {
                builder.Append(this.Identifier.ToString());
                builder.Append("->");
            }
            if (this.Property != null)
            {
                builder.Append(this.Property.ToString());
            }
            else
            {
                builder.Append("{EMPTY}");
            }
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ConditionPropertyExpression);
        }

        public virtual bool Equals(ConditionPropertyExpression other)
        {
            if (other == null)
            {
                return false;
            }
            if (object.ReferenceEquals(this, other))
            {
                return true;
            }
            if (!object.Equals(this.Identifier, other.Identifier))
            {
                return false;
            }
            if (!object.Equals(this.Property, other.Property))
            {
                return false;
            }
            return true;
        }
    }
}
