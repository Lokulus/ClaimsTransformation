using ClaimsTransformation.Language.Parser;
using System.Collections.Generic;
using System.Text;

namespace ClaimsTransformation.Language.DOM
{
    public class AggregateConditionExpression : ConditionExpression
    {
        public AggregateConditionExpression(LiteralExpression identifier, LiteralExpression name, IEnumerable<BinaryExpression> expressions) : this(identifier, name, expressions, null, null)
        {

        }

        public AggregateConditionExpression(LiteralExpression identifier, LiteralExpression name, IEnumerable<BinaryExpression> expressions, LiteralExpression @operator, LiteralExpression value) : base(identifier, expressions)
        {
            this.Name = name;
            this.Operator = @operator;
            this.Value = value;
        }

        public LiteralExpression Name { get; private set; }

        public LiteralExpression Operator { get; private set; }

        public LiteralExpression Value { get; private set; }

        public override int GetHashCode()
        {
            var hashCode = base.GetHashCode();
            unchecked
            {
                if (this.Name != null)
                {
                    hashCode += this.Name.GetHashCode();
                }
                if (this.Operator != null)
                {
                    hashCode += this.Operator.GetHashCode();
                }
                if (this.Value != null)
                {
                    hashCode += this.Value.GetHashCode();
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
            }
            else
            {
                builder.Append("{EMPTY}");
            }
            builder.Append(Terminals.COLON);
            if (this.Name != null)
            {
                builder.Append(this.Name.ToString());
            }
            builder.Append(Terminals.O_BRACKET);
            builder.Append(Terminals.O_SQ_BRACKET);
            if (this.Expressions != null)
            {
                var first = true;
                foreach (var expression in this.Expressions)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        builder.Append(Terminals.COMMA);
                    }
                    builder.Append(expression.ToString());
                }
            }
            builder.Append(Terminals.C_SQ_BRACKET);
            builder.Append(Terminals.C_BRACKET);
            if (this.Operator != null)
            {
                builder.Append(this.Operator.ToString());
            }
            if (this.Value != null)
            {
                builder.Append(this.Value.ToString());
            }
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
            if (!object.Equals(this.Operator, other.Operator))
            {
                return false;
            }
            if (!object.Equals(this.Value, other.Value))
            {
                return false;
            }
            return true;
        }
    }
}
