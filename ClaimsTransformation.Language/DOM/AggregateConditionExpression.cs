using ClaimsTransformation.Language.Parser;
using System.Collections.Generic;
using System.Text;

namespace ClaimsTransformation.Language.DOM
{
    public class AggregateConditionExpression : ConditionExpression
    {
        protected AggregateConditionExpression(LiteralExpression identifier, LiteralExpression name, IEnumerable<BinaryExpression> expressions) : base(identifier, expressions)
        {
            this.Name = name;
        }

        public LiteralExpression Name { get; private set; }

        public override int GetHashCode()
        {
            var hashCode = base.GetHashCode();
            unchecked
            {
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
            if (this.Name != null)
            {
                builder.Append(this.Name.ToString());
            }
            builder.Append(Terminals.O_BRACKET);
            builder.Append(base.ToString());
            builder.Append(Terminals.C_BRACKET);
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as AggregateConditionExpression);
        }

        public virtual bool Equals(AggregateConditionExpression other)
        {
            if (!base.Equals(other))
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
