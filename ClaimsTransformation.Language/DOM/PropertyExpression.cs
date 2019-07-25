using System.Text;

namespace ClaimsTransformation.Language.DOM
{
    public class PropertyExpression : Expression
    {
        public PropertyExpression(LiteralExpression source, LiteralExpression name)
        {
            this.Source = source;
            this.Name = name;
        }

        public LiteralExpression Source { get; private set; }

        public LiteralExpression Name { get; private set; }

        public override ExpressionType Type
        {
            get
            {
                return ExpressionType.Propery;
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
                if (this.Name != null)
                {
                    hashCode += this.Name.GetHashCode();
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
            if (this.Name != null)
            {
                builder.Append(this.Name.ToString());
            }
            else
            {
                builder.Append("{EMPTY}");
            }
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as PropertyExpression);
        }

        public virtual bool Equals(PropertyExpression other)
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
            if (!object.Equals(this.Name, other.Name))
            {
                return false;
            }
            return true;
        }
    }
}
