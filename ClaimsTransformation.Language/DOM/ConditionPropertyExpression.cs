using System.Text;

namespace ClaimsTransformation.Language.DOM
{
    public class ConditionPropertyExpression : Expression
    {
        public ConditionPropertyExpression(LiteralExpression source, ClaimPropertyExpression property)
        {
            this.Source = source;
            this.Property = property;
        }

        public LiteralExpression Source { get; private set; }

        public ClaimPropertyExpression Property { get; private set; }

        public override bool IsStatic
        {
            get
            {
                return false;
            }
        }

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
                if (this.Source != null)
                {
                    hashCode += this.Source.GetHashCode();
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
            if (this.Source != null)
            {
                builder.Append(this.Source.ToString());
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
            if (!object.Equals(this.Source, other.Source))
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
